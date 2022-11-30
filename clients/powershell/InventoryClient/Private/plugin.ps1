
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
  capacities
}
"@

$script:PluginQuery=@"
query plugins(`$first: Int `$after: String) {
  plugins(first: `$first  after: `$after includeDeprecated: false includeAllEntitites: false) {
    pageInfo { ... pageInfo }
    totalCount
    edges {
      cursor
      node {... pluginDto }
    }
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
  capacities
}
"@