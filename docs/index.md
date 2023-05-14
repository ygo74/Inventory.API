---
layout: default
title: Overview
nav_order: 1
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

## Goals

Implement an infrastructure inventory across all datacenters either on premise or in the cloud. This application will contain the expected state of all resources for an entreprise. It can be consumed by provisioning tools to automate the resources deployment and configuration.

This inventory is built by following a microservice architectural style. It is a main block of the [Devops platform](https://ygo74.github.io/).

This repository has the following goals:

- Build an application with dotnet based on a microservice pattern
- Deploy the application in a Kubernetes platform
- Provide a set of guidelines to develop and deploy a dotnet application

## Dynamic Inventory presentation

![Microservices Architecture](./assets/images/microservices-architecture.png)

## Application status

- ![License](https://img.shields.io/github/license/ygo74/Inventory.API)
- [![Known Vulnerabilities](https://snyk.io/test/github/ygo74/Inventory.API/badge.svg)](https://snyk.io/test/github/ygo74/Inventory.API)

| Service | Build status | Tests status | Code coverage | Deployment status |
|:------- |:------------:|:------------:|:-------------:|:-----------------:|
| Configuration | [![Build Status](https://dev.azure.com/ygo74/iac/_apis/build/status%2Fconfiguration-api-ci?branchName=master)](https://dev.azure.com/ygo74/iac/_build/latest?definitionId=34&branchName=master) | ![Tests status](https://img.shields.io/azure-devops/tests/ygo74/iac/34?compact_message) | ![Code coverage](https://img.shields.io/azure-devops/coverage/ygo74/iac/34) |[![Build Status](https://dev.azure.com/ygo74/iac/_apis/build/status%2Fconfiguration-api-cd?branchName=master)](https://dev.azure.com/ygo74/iac/_build/latest?definitionId=35&branchName=master) |

## Resources

- <https://learn.microsoft.com/en-us/azure/architecture/microservices/>{:target="_blank"}
- <https://learn.microsoft.com/en-us/azure/architecture/microservices/model/domain-analysis>{:target="_blank"}
