#!/usr/bin/env bash
set -euo pipefail
set -x

IMAGE_REPOSITORY=${1:-demo}
IMAGE_TAG=${2:-0.0.1}
CHART=dotnet-api
NAMESPACE=demo
DNS_NAME=foo.bar

helm upgrade --install "${CHART}" ../charts/"${CHART}" \
  --create-namespace \
  --namespace "${NAMESPACE}" \
  --set "replicaCount=1" \
  --set "image.pullPolicy=Never" \
  --set "image.tag=${IMAGE_TAG}" \
  --set "image.repository=${IMAGE_REPOSITORY},image.tag=${IMAGE_TAG}"
