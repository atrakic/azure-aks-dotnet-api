#!/usr/bin/env bash
set -euo pipefail

CHART_NAME=${1:?CHART_NAME must be set}
IMAGE_REPO=${2:?IMAGE_REPO must be set}
IMAGE_TAG=${3:?IMAGE_TAG must be set}

shift 3

helm upgrade --install "${CHART_NAME}" ./charts/"${CHART_NAME}" \
  --atomic \
  --set image.repository="${IMAGE_REPO}" \
  --set image.tag="${IMAGE_TAG}" "$@"
