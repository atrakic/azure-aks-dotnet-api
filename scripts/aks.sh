#!/usr/bin/env bash
set -euo pipefail

random=$RANDOM
readonly NAME=demo-$random

export RESOURCE_GROUP_NAME=$NAME
export AKS_NAME=$NAME
export ACR_NAME=$NAME

az login
az provider register -n Microsoft.Network
az provider register -n Microsoft.Storage
az provider register -n Microsoft.Compute
az provider register -n Microsoft.ContainerService

echo "Creating resource group..."
az group create -n "$RESOURCE_GROUP_NAME" -l eastus

# https://docs.microsoft.com/en-us/azure/aks/http-application-routing
echo "Creating AKS cluster..."
az aks create \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --name "$AKS_NAME" \
  --node-count 1 \
  --enable-addons http_application_routing \
  --dns-name-prefix "$AKS_NAME" \
  --enable-managed-identity \
  --generate-ssh-keys \
  --node-vm-size Standard_B2s

echo "Obtaining AKS credentials..."
az aks get-credentials -n "$AKS_NAME" -g "$RESOURCE_GROUP_NAME"

echo "Creating ACR..."
az acr create -n $ACR_NAME -g $RESOURCE_GROUP_NAME --sku basic
az acr update -n $ACR_NAME -g $RESOURCE_GROUP_NAME --admin-enabled true
ACR_USERNAME=$(az acr credential show -n $ACR_NAME -g $RESOURCE_GROUP_NAME --query "username" -o tsv)
ACR_PASSWORD=$(az acr credential show -n $ACR_NAME -g $RESOURCE_GROUP_NAME --query "passwords[0].value" -o tsv)

az aks update \
    --name "$AKS_NAME" \
    --resource-group "$RESOURCE_GROUP_NAME" \
    --attach-acr "$ACR_NAME"

# AKS DNS Zone Name:
DNS_NAME=$(az network dns zone list -g $RESOURCE_GROUP_NAME -o json --query "[?contains(resourceGroup,'$RESOURCE_GROUP_NAME')].name" -o tsv)

echo "-- Infra installation completed: --"
echo "RESOURCE_GROUP_NAME=$RESOURCE_GROUP_NAME"
echo "ACR_NAME=$ACR_NAME"
echo "ACR_USERNAME=$ACR_USERNAME"
echo "ACR_PASSWORD=$ACR_PASSWORD"
echo "AKS_NAME=$AKS_NAME"
echo "DNS_NAME=$DNS_NAME"
