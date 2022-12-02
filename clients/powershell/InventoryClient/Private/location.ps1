
$script:CreateLocationMutation=@"
mutation createLocation(`$input: CreateLocationRequestInput!) {
  createLocation(input: `$input) {
    data { ... locationDto }
    errors {
     message
     __typename
   }
  }
}
"@

$script:PluginLocation=@"
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



$script:LocationDtoFragment=@"
fragment locationDto on LocationDto {
  name
  description
  regionCode
  countryCode
  cityCode
  deprecated
  startDate
  endDate
  created
  createdBy
  lastModified
  lastModifiedBy
}
"@