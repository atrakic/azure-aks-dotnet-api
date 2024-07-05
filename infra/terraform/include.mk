RG := $(shell terraform output -raw resource_group_name)
AKS := $(shell terraform output -raw kubernetes_cluster_name)

configure-kubectl:
	az aks get-credentials --resource-group $(RG) --name $(AKS)

generate-azure-portal-link:
	az aks browse --resource-group $(RG) --name $(AKS)
