variable "client_secret" {}

terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>2.0"
    }
  }
}
provider "azurerm" {
  features {}

  subscription_id = "62fab4e9-2e08-4c12-8bbf-2c263b4b6061"
  client_id       = "53f9b906-3d9b-43ec-8bf0-170f0b36a526"
  client_secret   = var.client_secret
  tenant_id       = "f8a45619-86c0-4b14-8e1c-96a4582df2ce"
}
resource "azurerm_resource_group" "rg" {
  name     = "DefaultResourceGroup-WUK"
  location = "ukwest"
}
resource "azurerm_app_service_plan" "MockServicePlan" {
  name = "MockServicePlan"
  location = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku {
    size = "F1"
    tier = "Free"
  }
}
resource "azurerm_app_service" "MockApiService" {
  name = "MockApiService"
  location = azurerm_app_service_plan.MockServicePlan.location
  resource_group_name = azurerm_app_service_plan.MockServicePlan.resource_group_name
  app_service_plan_id = azurerm_app_service_plan.MockServicePlan.id
}