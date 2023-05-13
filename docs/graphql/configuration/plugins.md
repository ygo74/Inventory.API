query getPlugins ($first: Int $after: String) {
  plugins(first: $first  after: $after includeDeprecated: false includeAllEntitites: false) {
    pageInfo { ... cursorPageInfo }
    totalCount
    edges
    {
      cursor
      node { ... pluginDto }
    }
  }
}

query getPlugins2 {
  plugins2 {
    pageInfo {
      hasNextPage
      hasPreviousPage
    }
    totalCount
    items { ... pluginDto }
  }
}

mutation createPlugin{
  createPlugin(input: {
    code: "T2"
    name: "T2"
    version: "1.0"
    path: "D:/devel/github/ansible_inventory/Services/plugins/Azure/Inventory.Plugins.Azure/bin/Debug/net6.0/Inventory.Plugins.Azure.dll"
  }) {
    plugin { ... pluginDto}
  }
}

fragment cursorPageInfo on PageInfo
{
      hasNextPage
      hasPreviousPage
      startCursor
      endCursor
}

fragment pluginDto on PluginDto
{
  id
  name
  code
  version
  deprecated
  startDate
  endDate
  capacities
  created
  createdBy
  lastModified
  lastModifiedBy
}