#!/bin/bash

APP=${1:-app_app_1}
PORT=3000
IP_ADDRESS=0.0.0.0

run_in_docker() {
  IP_ADDRESS=$(docker inspect \
    -f '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' \
    "$APP"
  )

  if [ -z "$IP_ADDRESS" ]; then
    echo "Could not find $APP container." >&2
    exit 1
  fi
  return "$IP_ADDRESS"
}

curl -sSL "$IP_ADDRESS":"$PORT"|jq -r "."
curl -sSL "$IP_ADDRESS":"$PORT"/users|jq -r "."
echo
