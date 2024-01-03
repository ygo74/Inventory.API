$script:CreateCredentialMutation=@"
mutation createCredential(`$input: CreateCredentialRequestInput)
{
 createCredential(input: `$input) {
   data { ...credentialDto }
   errors { ...error }
 }
}
"@

$script:UpdateCredentialMutation=@"
mutation updateCredential(`$input: UpdateCredentialRequestInput)
{
  updateCredential(input: `$input) {
   data { ...credentialDto }
   errors { ...error }
 }
}
"@

$script:RemoveCredentialMutation=@"
mutation removeCredential(`$input: RemoveCredentialRequestInput)
{
  removeCredential(input: `$input) {
   data { ...credentialDto }
   errors { ...error }
 }
}
"@


$script:GetCredentialByNameQuery=@"
query getCredential(`$name: String)
{
  credential:credentialByName(name: `$name) {
    data { ... credentialDto }
    errors { ... error }
  }
}
"@

$script:GetCredentialByIdQuery=@"
query getCredential(`$id: Int!)
{
  credential(id: `$id) {
    data { ... credentialDto }
    errors { ... error }
  }
}
"@

$script:FindCredentialQuery=@"
query findCredentials(`$query: GetCredentialsRequestInput!, `$skip: Int, `$take: Int)
{
    credentials(request: `$query, skip: `$skip, take: `$take) {
    pageInfo { ... pageInfo }
    totalCount
    items { ... credentialDto }
  }
}
"@



$script:CredentialDtoFragment=@"
fragment credentialDto on CredentialDto
{
    id
    name
    description
    propertyBag
    created
    createdBy
    lastModified
    lastModifiedBy
}
"@



