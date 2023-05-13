---
layout: default
title: Continuous Deployment
parent: Delivery pipeline
nav_order: 3
has_children: false
---

https://learn.microsoft.com/en-us/azure/aks/devops-pipeline?pivots=pipelines-yaml

## Standard deployment into Kubernetes

### Create deployment file

1. Retrieve information on images

    ``` bash
    az acr login --name aksbootstrap
    az acr repository list -n aksbootstrap
    az acr repository show -n aksbootstrap --repository inventoryconfigurationapi
    az acr repository show-tags -n aksbootstrap --repository inventoryconfigurationapi

    # Attach using acr-name
    az aks get-credentials --name aksbootstrap --resource-group rg-aks-bootstrap-networking-spoke
    az aks update -n aksbootstrap -g rg-aks-bootstrap-networking-spoke --attach-acr aksbootstrap

    ```

2. Create the deployment file

3. Deploy on Kubernetes

    3.1 Deployment

        ``` bash
        # Create namespace inventory
        $namespace = "inventory"
        $releaseName = "$namespace-ingress"
        kubectl create namespace $namespace

        kubectl apply -f builds/kubernetes/temp/deployment.yml --namespace inventory
        kubectl get deployments

        kubectl logs configuration-api-deployment-fd99558-4dn7z -n inventory -c postgres-image
        ```

    3.2 Service Cluster IP

        ``` bash
        kubectl apply -f .\builds\kubernetes\temp\service-clusterip.yml --namespace inventory
        kubectl describe service configuration-api-svc --namespace inventory
        ```

    3.3 AGIC...

        ``` bash
        # Create application gateway
        az network application-gateway create -n ag-aks -l francecentral -g rg-aks-bootstrap-networking-hub --sku Standard_v2 --public-ip-address pi-inventory-gateway --vnet-name vnet-hub --subnet gateway-subnet

        # enable application gateway into aks cluster (module AGIC)
        # https://learn.microsoft.com/fr-fr/azure/application-gateway/tutorial-ingress-controller-add-on-existing?toc=https%3A%2F%2Flearn.microsoft.com%2Ffr-fr%2Fazure%2Faks%2Ftoc.json&bc=https%3A%2F%2Flearn.microsoft.com%2Ffr-fr%2Fazure%2Fbread%2Ftoc.json
        $appgwId=$(az network application-gateway show -n ag-aks -g rg-aks-bootstrap-networking-hub -o tsv --query "id")
        az aks enable-addons -n aksbootstrap -g rg-aks-bootstrap-networking-spoke -a ingress-appgw --appgw-id $appgwId

        az aks update -n aksbootstrap -g rg-aks-bootstrap-networking-spoke --enable-managed-identity
        ```

### Create Helm chart

TODO
