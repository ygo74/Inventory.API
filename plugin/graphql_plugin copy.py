from gql import gql, Client, AIOHTTPTransport

# Select your transport with a defined url endpoint
transport = AIOHTTPTransport(url="https://192.168.1.10:32778/graphql")

# Create a GraphQL client using the defined transport
client = Client(transport=transport, fetch_schema_from_transport=True)

# Provide a GraphQL query
query = gql(
    """
    query {
	    group(groupName: "windows")
        {
            name
            parents {
                name
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
"""
)

# Execute the query on the transport
result = client.execute(query)
print(result["group"]["name"])
# for group_name in result["group"]["children"]:
#     print(group_name)