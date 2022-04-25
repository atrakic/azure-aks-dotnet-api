.DEFAULT_GOAL := install

IMAGE := demo
VERSION := 0.0.1

install:
	./local-helm.sh $(IMAGE) $(VERSION)

local-minikube:
	minikube start
	minikube addons enable ingress
	kubectl config use-context minikube
	kubectl wait --namespace ingress-nginx \
		--for=condition=ready pod \
		--selector=app.kubernetes.io/component=controller \
		--timeout=90s
	
	eval $(minikube docker-env)
	pushd ../src/
	docker build -t $(IMAGE):$(VERSION) .
	minikube image ls
	pod=test-$(IMAGE)
	kubectl run $(pod) --image=$(IMAGE):$(VERSION) --image-pull-policy=Never
	kubectl get pods
	kubectl logs pod/$(pod)
	kubectl delete pod/$(pod)
	popd
	eval $(minikube docker-env -u)

clean:
	echo "You are about to delete demo cluster."
	echo "Are you sure? (Press Enter to continue or Ctrl+C to abort) "
	read _
	minikube delete
