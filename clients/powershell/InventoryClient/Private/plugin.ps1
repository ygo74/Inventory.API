
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