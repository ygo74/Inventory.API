
$script:CreatePluginMutation=@"
mutation createPlugin(`$input: CreatePluginRequestInput)
{
  createPlugin(input: `$input) {
      data { ...pluginDto }
      errors { ...error }
  }
}
"@

$script:UpdatePluginMutation=@"
mutation updatePlugin(`$input: UpdatePluginRequestInput)
{
  updatePlugin(input: `$input) {
      data { ...pluginDto }
      errors { ...error }
  }
}
"@

$script:RemovePluginMutation=@"
mutation removePlugin(`$input: RemovePluginRequestInput)
{
  removePlugin(input: `$input) {
      data { ...pluginDto }
      errors { ...error }
  }
}
"@

$script:GetPluginByCodeQuery=@"
query getPlugin(`$code: String)
{
  plugin:pluginByCode(code: `$code) {
    data { ... pluginDto }
    errors { ... error }
  }
}
"@

$script:GetPluginByIdQuery=@"
query getPlugin(`$id: Int!)
{
  plugin(id: `$id) {
    data { ... pluginDto }
    errors { ... error }
  }
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

$script:pluginDtoFragment=@"
fragment pluginDto on PluginDto
{
  id
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