
query datacenterByName($getPlunginEndpoint: Boolean = false) {
  datacenter:datacenterByName(name: "Azure France Central") {
    data {
      ... DatacenterDto
      plugins @include(if: $getPlunginEndpoint) { ... DatacenterPlugins }
    }

  }
}

query datacenterByCode($getPlunginEndpoint: Boolean = false) {
  datacenter:datacenterByCode(code: "AZR-FR-CENTRAL") {
    data {
      ... DatacenterDto
      plugins @include(if: $getPlunginEndpoint) { ... DatacenterPlugins }
    }

  }
}

query datacenterById($getPlunginEndpoint: Boolean = false) {
  datacenter(id: 1) {
    data {
      ... DatacenterDto
      plugins @include(if: $getPlunginEndpoint) { ... DatacenterPlugins }
    }
  }
}

query datacenters {
  datacenters (request: {
     includeDeprecated: false
     allEntities: false
    }
  ) {
   pageInfo {
     hasNextPage
   }
   totalCount
   nodes { ... DatacenterDto }
  }
}

query searchDatacenters {
  datacenters: searchDatacenters {
    pageInfo {
      hasNextPage
      hasPreviousPage
      startCursor
      endCursor
    }
    totalCount
    edges {
      cursor
      node { ... DatacenterDto }
    }
  }
}

mutation createDatacenter {
  createDatacenter(input: {
    code: "AA5"
    name: "Datacenter 5"
    inventoryCode: "dc"
    datacenterType: CLOUD
    cityCode: "PAR"
    countryCode: "FR"
    regionCode: "EMEA"

  })
  {
    data { ... DatacenterDto }
    errors { ... error}
  }
}


mutation updateDatacenter {
  updateDatacenter(input: {
    id: 1
    datacenterType: ON_PREMISE
  }) {
    data { ... DatacenterDto }
    errors { ... error }
  }
}


mutation setDataCenterPluginEnpoint  {
  setDataCenterPluginEnpoint(
    credentialName: "Azure Credential"
    datacenterCode: "AZR-FR-CENTRAL"
    pluginCode: "Azure.Plugin")
  {
      data { ... DatacenterPlugins }
      errors { ... error }
  }
}

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


fragment DatacenterPlugins on DatacenterPlugins {
  id
  datacenterId
  credentialName
  credentialDescription
  credentialPropertyBag
  pluginName
  pluginCode
  pluginVersion
  pluginPath
  pluginEndpointPropertyBag
}


fragment error on ApiError
{
    __typename
    ... genericError
    ... validationError
    ... unAuthorisedError
    ... notFoundError
}
fragment genericError on GenericApiError
{
  message
}
fragment validationError on ValidationError
{
  message
}
fragment unAuthorisedError on UnAuthorisedError
{
  message
}
fragment notFoundError on NotFoundError
{
  message
}
