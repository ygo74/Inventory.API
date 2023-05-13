
$script:CreateLocationMutation=@"
mutation createLocation(`$input: CreateLocationRequestInput!) {
  createLocation(input: `$input) {
    data { ... locationDto }
    errors {
      __typename
      ... error
      ... genericError
      ... validationError
      ... genericError
    }
  }
}
"@

$script:UpdateLocationMutation=@"
mutation updateLocation(`$input: UpdateLocationRequestInput)
{
  updateLocation(input: `$input) {
    data { ... locationDto }
    errors { ... error }
  }
}
"@

$script:RemoveLocationMutation=@"
mutation removeLocation(`$input: DeleteLocationRequestInput!)
{
  removeLocation(input: `$input) {
    data { ... locationDto }
    errors { ... error }
  }
}
"@


$script:FindLocationQuery=@"
query findLocations(`$query: GetLocationRequestInput!, `$after: String, `$before: String, `$first: Int, `$last: Int)
{
  locations(request: `$query, after: `$after, before: `$before, first: `$first, last: `$last) {
    pageInfo { ... pageInfo }
    totalCount
    edges
    {
      cursor
      node {... locationDto}
    }
  }
}
"@

$script:GetLocationByNameQuery=@"
query getLocation(`$name: String)
{
  locationByName(name: `$name) {
    data { ... locationDto }
    errors {
     message
     __typename
    }
  }
}
"@

$script:GetLocationByIdQuery=@"
query getLocation(`$id: Int!)
{
  location(id: `$id) {
    data { ... locationDto }
    errors {
     message
     __typename
    }
  }
}
"@


$script:LocationDtoFragment=@"
fragment locationDto on LocationDto {
  id
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