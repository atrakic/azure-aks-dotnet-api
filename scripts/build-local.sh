#!/usr/bin/env bash
set -euo pipefail

IMAGE="${1:?IMAGE must be set}"
VERSION="${2:?VERSION must be set}"

eval "$(minikube docker-env)"
pushd "$(git rev-parse --show-toplevel)"/src  > /dev/null
docker build -t "${IMAGE}":"${VERSION}" .
minikube image ls
pod="test-${IMAGE}"
kubectl run "${pod}" --image="${IMAGE}":"${VERSION}" --image-pull-policy=Never
kubectl wait --for=condition=Ready pods --timeout=300s --all
kubectl get pods
kubectl logs pod/"${pod}"
kubectl delete pod/"${pod}"
eval "$(minikube docker-env -u)"
popd  > /dev/null
