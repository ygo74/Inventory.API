---
layout: default
title: Packaging
parent: Delivery pipeline
nav_order: 3
has_children: false
---

<details open markdown="block">
  <summary>
    Table of contents
  </summary>
  {: .text-delta }
1. TOC
{:toc}
</details>

## Container Image

## Helm chart

1. Create Helm chart

    ``` powershell
    helm create devices-api

    ```

1. Define repository

    set repository value into images dictionary in values.yaml

    ``` yaml
    image:
      repository: aksbootstrap.azurecr.io/dynamic-inventory/devices-api

    ```

kubectl create namespace dynamic-inventory
# kubectl label namespace dynamic-inventory cert-manager.io/disable-validation=true

helm create inventory-api

helm upgrade inventoryconfigurationapi .\configuration-api\ `
     --install `
     --values .\configuration-api\values.yaml `
     --namespace dynamic-inventory `
     --set image.tag=915 `
     --set postgres.host=postgresql.postgresql `
     --set postgres.database=ConfigurationDB `
     --set postgres.user=configurationdb_rw `
     --set postgres.password=test



https://anthonychu.ca/post/aspnet-core-appsettings-secrets-kubernetes/

kubectl create secret generic secret-appsettings --from-file=./../temp/appsettings.secrets.json


EF :
https://dotnetthoughts.net/run-ef-core-migrations-in-azure-devops/
https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli

dotnet ef migrations script --output ..\databases\configuration\configurationDB.sql --idempotent -s .\configuration\Inventory.Configuration.Api\ -p .\configuration\Inventory.Configuration.Infrastructure\

## Known issues

1. Semantic version for docker

2. Semantic version for helm

    ``` bash
    OCI artifact references (e.g. tags) do not support the plus sign (+). To support
    storing semantic versions, Helm adopts the convention of changing plus (+) to
    an underscore (_) in chart version tags when pushing to a registry and back to
    a plus (+) when pulling from a registry.

    ```