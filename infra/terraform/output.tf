output "resource_group_name" {
  value = azurerm_resource_group.demo.name
}

output "kubernetes_cluster_name" {
  value = azurerm_kubernetes_cluster.demo.name
}

/*
output "kube_config" {
   value = azurerm_kubernetes_cluster.demo.kube_config_raw
}
*/
