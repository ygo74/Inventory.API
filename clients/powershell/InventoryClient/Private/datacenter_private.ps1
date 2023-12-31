$script:CreateDatacenterMutation=@"
mutation createDatacenter(`$input: CreateDatacenterRequestInput!) {
    createDatacenter(input: `$input) {
    data { ... DatacenterDto }
    errors { ... error }
  }
}
"@

$script:UpdateDatacenterMutation=@"
mutation updateDatacenter(`$input: UpdateDatacenterRequestInput)
{
  updateDatacenter(input: `$input) {
    data { ... DatacenterDto }
    errors { ... error }
  }
}
"@


$script:GetDatacenterByNameQuery=@"
query getDatacenter(`$name: String)
{
  datacenter:datacenterByName(name: `$name) {
    data { ... DatacenterDto }
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

$script:GetDatacenterByCodeQuery=@"
query getDatacenter(`$code: String)
{
  datacenter:datacenterByCode(code: `$code) {
    data { ... DatacenterDto }
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


$script:GetDatacenterByIdQuery=@"
query getDatacenter(`$id: Int!)
{
  datacenter(id: `$id) {
    data { ... DatacenterDto }
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

$script:FindDatacenterQuery=@"
query findDatacenters(`$query: GetDatacenterRequestInput!, `$after: String, `$before: String, `$first: Int, `$last: Int)
{
    datacenters(request: `$query, after: `$after, before: `$before, first: `$first, last: `$last) {
    pageInfo { ... pageInfo }
    totalCount
    edges
    {
      cursor
      node {... DatacenterDto}
    }
  }
}
"@


$script:DatacenterDtoFragment=@"
fragment DatacenterDto on DatacenterDto {
  id
  name
  Code
  inventoryCode
  datacenterType
  description
  locationName
  locationRegionCode
  locationCountryCode
  locationCityCode
  deprecated
  startDate
  endDate
  created
  createdBy
  lastModified
  lastModifiedBy
}
"@