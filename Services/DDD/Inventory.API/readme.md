
Graphql :
https://github.com/fiyazbinhasan/GraphQLCore/tree/Part_X_DataLoader
https://github.com/graphql-dotnet/graphql-dotnet

1) Injection Middleware graphql
https://graphql-dotnet.github.io/docs/guides/migration3

add app.UseGraphQLMiddleware(); in configure method

2) define Type

3) define Query

4) define mutation


2) Create schema




https://volosoft.com/Blog/Building-GraphQL-APIs-with-ASP.NET-Core


## use Query

### Configurations
query allLocations
{
	locations
	{
		name
		cityCode
		countryCode
	}
}

query allTrustLevels{
	trustLevels{
		name
		code
	}
}

# Inventory
query {
	servers {
		name
		operatingSystem
	}
}

## Use mutation :
mutation createServer($server: ServerInput!) {
   createServer(server: $server)
	{
		name
		operatingSystem
	}
}

variables:
{
  "server":
  {
    "name": "test10",
    "networkLocation": "value",
    "operatingSystem": "LINUX"
  }
}