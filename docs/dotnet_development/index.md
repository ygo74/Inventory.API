---
layout: default
title: Dotnet development
nav_order: 2
has_children: true
mermaid: true
---

## How to develop with dotnet framework

```mermaid
flowchart LR
  subgraph Entities
    direction TB
    subgraph Domain
        direction TB
        Model --> Specification
        Model --> Filter
    end
    subgraph Infrastructure
        direction TB
        Configuration --> Repository
    end
  end
  subgraph Application
    direction TB
    Dto --> Validators
    Dto --> Handler
  end
  subgraph UnitTests
    direction TB
    ro(Tests domain) --> ro(Test Application)
  end
  Domain --> Infrastructure
  Application --> Entities
  Application --> UnitTests
  Entities --> Application
```

```mermaid
graph TB
    sq[Square shape] --> ci((Circle shape))

    subgraph A
        od>Odd shape]-- Two line<br/>edge comment --> ro
        di{Diamond with <br/> line break} -.-> ro(Rounded<br>square<br>shape)
        di==>ro2(Rounded square shape)
    end

    %% Notice that no text in shape are added here instead that is appended further down
    e --> od3>Really long text with linebreak<br>in an Odd shape]

    %% Comments after double percent signs
    e((Inner / circle<br>and some odd <br>special characters)) --> f(,.?!+-*ز)

    cyr[Cyrillic]-->cyr2((Circle shape Начало));

     classDef green fill:#9f6,stroke:#333,stroke-width:2px;
     classDef orange fill:#f96,stroke:#333,stroke-width:4px;
     class sq,e green
     class di orange
```

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
