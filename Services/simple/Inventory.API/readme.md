
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
query {
	servers {
		name
		operatingSystem
	}
}

query {
	group(groupName: "windows")
	{
		name
		parents {
			name
			parents {
				name
			}
		}
		children {
			name
			servers {
				hostName
			}		  
		}
		servers {hostName}
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

EF CORE : 
https://www.entityframeworktutorial.net/efcore/saving-data-in-disconnected-scenario-in-ef-core.aspx


1. Migration
prerequisites : 
  * Install command cli tools   
     dotnet tool install --global dotnet-ef
     dotnet add Inventory.API package Microsoft.EntityFrameworkCore.Design
  * Configure DbContext
     ```c#
	  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("host=postgres_image;port=32774;database=blogdb;username=bloguser;password=bloguser");
            }
        }
	 ```

Create Initial Database description :
  * dotnet ef migrations add InitialCreate -p Inventory.API -s Inventory.API

Generate sql scripts :
  * dotnet ef migrations script -p Inventory.API -s Inventory.API