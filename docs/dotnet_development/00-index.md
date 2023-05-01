---
layout: default
title: Dotnet development
nav_order: 2
has_children: true
mermaid: true
---

## How to develop with dotnet framework

```mermaid
stateDiagram-v2
    direction LR
    [*] --> Domain
    Domain: Implement Domain
    state Domain {
        domain --> specification
        domain --> filter
        Specifications --> Infrastructure
        domain: Domain Entity
        Infrastructure: Map Infrastructure
    }
    Domain --> Application
    Application: Implement Application
    state Application {
        Dto --> Validators
        Validators --> Handlers
        Dto: Entity Dto
    }
    Application --> Domain
    Application --> Tests
    Tests: Unit Tests
    state Tests {
        domainTest --> domainApplication
    }
    Tests --> Application
    Tests --> Expose
    state Expose {
        direction TB
        Controllers --> Graphql
        Graphql --> Views
    }
    Expose --> [*]
```
