---
layout: default
title: Git branching strategy
parent: Dotnet development
nav_order: 1
has_children: false
mermaid: true
---

<details open markdown="block">
  <summary>
    Table of contents
  </summary>
  {: .text-delta }
1. TOC
{:toc}
</details>

``` mermaid
%%{init: { 'logLevel': 'debug', 'theme': 'default', 'gitGraph': {'showBranches': true, 'showCommitLabel':true,'mainBranchOrder': 2}} }%%
      gitGraph
        commit type:HIGHLIGHT tag: "v1.0.0"
        checkout main
        branch featureA order: 3
        commit id: "1.1.0-beta.1+featureA"
        commit id: "1.1.0-beta.2+featureA"
        checkout main
        branch featureB order: 4
        commit id: "1.1.0-beta.1+featureB"
        checkout main
        merge featureB id: "1.1.0-rc.1"
        commit
        branch featureC  order: 5
        commit id: "1.1.0-beta.1+featureC"
        checkout featureA
        commit id: "1.1.0-beta.3+featureA"
        checkout main
        merge featureA id: "1.1.0-rc.2"
        checkout featureC
        commit id: "1.1.0-beta.2+featureC"
        commit id: "1.1.0-beta.3+featureC"
        checkout main
        merge featureC id: "1.1.0-rc.3"
        branch release_1_1_0 order: 1
        checkout release_1_1_0
        commit id: "1.1.0"
```

## Sources

- <https://learn.microsoft.com/en-us/azure/devops/repos/git/git-branching-guidance?view=azure-devops>{:target="_blank"}
- <https://learn.microsoft.com/en-us/devops/develop/how-microsoft-develops-devops?source=recommendations>{:target="_blank"}
