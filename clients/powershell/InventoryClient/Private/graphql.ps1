function Invoke-InternalGraphql
{
    [CmdletBinding()]
    param(
        [System.Uri]
        $Uri,

        [string]
        $Query,

        [PsObject]
        $Variables
    )

    Begin
    {

    }
    End
    {

    }
    Process
    {

        try
        {
            $body = @{
                query = $Query
                variables = $Variables
            }

            $jsonBody = $body | ConvertTo-Json

            $result = Invoke-RestMethod -Uri $Uri -Method Post -Body $jsonBody -ContentType "application/json" -ErrorAction stop
            $result.data

        }
        catch
        {
            throw $_
        }

    }
}

function ConvertFrom-InternalGraphqlErrors
{
    [CmdletBinding()]
    param(
        [Object[]]
        $Errors
    )


    $groupbyErrors = $Errors | Group-Object -Property __typeName

    $Message = ""

    foreach($errorType in $groupbyErrors)
    {
        $Message += $errorType.Name
        foreach($errorMessage in $errorType.Group)
        {
            $Message += ("`r`n`t- {0}" -f $errorMessage.Message)
        }
    }

    $Message
}