---
layout: default
title: Continuous Deployment
parent: Delivery pipeline
nav_order: 3
has_children: false
---

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

    ``` bash
    kubectl apply -f builds/kubernetes/temp/deployment.yml
    kubectl get deployments 
    ```

### Create Helm chart

TODO
