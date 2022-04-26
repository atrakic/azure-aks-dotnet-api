#!/usr/bin/env bash
set -euo pipefail
set -x

IMAGE_REPOSITORY=${1:-demo}
IMAGE_TAG=${2:-0.0.1}
DNS_NAME=${3:-demo.chart}
NAMESPACE=${4:-demo}
CHART=${5:-dotnet-api}

#  --set "ingress.annotations.\nginx\.ingress\.kubernetes\.io/rewrite-target: /\$1" \
#  --set "ingress.hosts[0].host=${DNS_NAME}" \
helm upgrade --install "${CHART}" ./charts/"${CHART}" \
  --atomic \
  --create-namespace \
  --namespace "${NAMESPACE}" \
  --set "replicaCount=1" \
  --set "image.pullPolicy=Never" \
  --set "image.tag=${IMAGE_TAG}" \
  --set "ingress.enabled=true" \
  --set "ingress.annotations.kubernetes\.io/ingress\.class=addon-http-application-routing" \
  --set "image.repository=${IMAGE_REPOSITORY},image.tag=${IMAGE_TAG}"
