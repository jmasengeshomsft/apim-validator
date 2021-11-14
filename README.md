
## How to run the deploy the function app infrastructure in your Azure Subscription using Azure CLI


Configure the variables.

```bash
# Global
export RG_NAME=demo-deploy-button
export RG_REGION=centralus
export TEMPLATE_FILE=infrastructure/azuredeploy.json
export PARAMETER_FILE=infrastructure/azuredeploy.parameters.json

```

Create a resource group for this project

```bash
az group create --name $RG_NAME --location $RG_REGION
```

Deploy the function app with a storage account and app insights 

```bash
az deployment group create --name createFunctionApp -g $RG_NAME --template-file $TEMPLATE_FILE --parameters $PARAMETER_FILE
```

## How to run the deploy the function app infrastructure in your Azure Subscription using Deploy Button

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fjmasengeshomsft%2Fpoc-table-storage-api-deploy-button%2Fmain%2Finfrastructure%2Fazuredeploy.json)
