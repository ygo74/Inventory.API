kubectl create namespace dynamic-inventory
# kubectl label namespace dynamic-inventory cert-manager.io/disable-validation=true

helm create inventory-api

helm upgrade inventoryconfigurationapi .\configuration-api\ `
     --install `
     --values .\configuration-api\values.yaml `
     --namespace dynamic-inventory `
     --set image.tag=915 `
     --set postgres.host=postgresql.postgresql `
     --set postgres.database=ConfigurationDB `
     --set postgres.user=configurationdb_rw `
     --set postgres.password=test



https://anthonychu.ca/post/aspnet-core-appsettings-secrets-kubernetes/

kubectl create secret generic secret-appsettings --from-file=./../temp/appsettings.secrets.json


EF :
https://dotnetthoughts.net/run-ef-core-migrations-in-azure-devops/
https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli

dotnet ef migrations script --output ..\databases\configuration\configurationDB.sql --idempotent -s .\configuration\Inventory.Configuration.Api\ -p .\configuration\Inventory.Configuration.Infrastructure\
