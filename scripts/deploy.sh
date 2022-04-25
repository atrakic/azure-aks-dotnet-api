#!/usr/bin/env bash
set -euo pipefail

# https://docs.microsoft.com/en-us/azure/aks/http-application-routing

APP_NAME="${1:?APP_NAME must be set}"
DNS_NAME="${2:?DNS_NAME must be set}"
ACR_NAME="${3:?ACR_NAME must be set}"
NAMESPACE="${4:?NAMESPACE must be set}"
IMAGE_TAG="${5:-latest}"
IMAGE_PULL_POLICY="${6:-Always}"

# FIXME:
#  --set "ingress.hosts[0].host=${DNS_NAME},ingress.hosts[0].paths[0].path=/" \
helm upgrade \
  --install \
  --create-namespace \
  --namespace "${NAMESPACE}" \
  "${APP_NAME}" \
  ./charts/"${APP_NAME}" \
  --atomic \
  --wait \
  --set replicaCount=2 \
  --set ingress.enabled=true \
  --set ingress.annotations."kubernetes\.io/ingress\.class"=addon-http-application-routing \
  --set image.pullPolicy="${IMAGE_PULL_POLICY}" \
  --set image.repository="${ACR_NAME}" \
  --set image.tag="${IMAGE_TAG}"
