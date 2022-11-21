
$script:CreatePluginMutation=@"
mutation createPlugin(`$input: CreatePluginInput)
{
 createPlugin(input: `$input) {
   plugin {
    ...pluginDto
   }
   errors {
     message
     __typename
   }
 }
}

fragment pluginDto on PluginDto
{
  name
  code
  version
  deprecated
  startDate
  endDate
  created
  createdBy
  lastModified
  lastModifiedBy
  capacities {
    key
    value
  }
}
"@

$script:PluginQuery=@"
query plugin(`$first: Int `$order: [PluginDtoSortInput!]) {
  plugins(first: `$first order: `$order) {
    pageInfo { ... pageInfo }
    edges {
      cursor
      node {... pluginDto }
    }
    nodes { ... pluginDto }
  }
}
"@

# $script:PluginQuery=@"
# query plugin {
#   plugins {
#     ... pluginDto
#   }
# }
# "@


$script:pluginDtoFragment=@"
fragment pluginDto on PluginDto
{
  name
  code
  version
  deprecated
  startDate
  endDate
  created
  createdBy
  lastModified
  lastModifiedBy
  capacities {
    key
    value
  }
}
"@