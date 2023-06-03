---
layout: default
title: Delivery pipeline
nav_order: 4
has_children: true
---

## Goals

Build, tests, package and deploy applications with build servers.

Application's components are built and deploy thanks to a dedicated pipeline :

- Continuous integration pipeline

    It is responsible to quickly validate developments commits by building and executing the first level of unit tests.

- Continuous deployment pipeline

    It is responsible to deploy application component to its execution platform

Delivery an application to its execution environment has also to tackle some concerns that developers don't have on their own development environment :

- Apply different configuration according the execution environment
- Manage secrets
- Host services behind a reverse proxy

## Prerequisites

- ✅ [ACR deployed](https://ygo74.github.io/azure/03-acr/00-index.html)
- ✅ [AKS deployed](https://ygo74.github.io/azure/04-aks/00-index.html)
- ✅ Azure Devops Services account

## Sources

- <https://learn.microsoft.com/en-us/azure/architecture/microservices/ci-cd-kubernetes>{:target="_blank"}
- <https://learn.microsoft.com/en-us/azure/architecture/solution-ideas/articles/dev-test-microservice>{:target="_blank"}



https://andrewlock.net/deploying-asp-net-core-applications-to-kubernetes-part-8-running-database-migrations-using-jobs-and-init-containers/


https://github.com/GitTools/actions/blob/main/docs/examples/azure/gitversion/execute/usage-examples.md
https://gitversion.net/docs/usage/cli/installation
dotnet tool install --global GitVersion.Tool --version 5.*


https://github.com/gbaeke/go-template/tree/main/azdo

