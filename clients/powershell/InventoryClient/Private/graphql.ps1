
$script:PaginationInfo=@"
fragment pageInfo on PageInfo
{
  hasNextPage
  hasPreviousPage
  startCursor
  endCursor
}
"@


function Invoke-InternalGraphql
{
    [CmdletBinding()]
    param(
        [System.Uri]
        $Uri,

        [string]
        $Query,

        [AllowNull()]
        [PsObject]
        $Variables = $null
    )

    Begin
    {
        $startFunction = Get-Date
        Trace-StartFunction -CommandName $MyInvocation.MyCommand.Name
    }
    End
    {
        $endFunction = Get-Date
        Trace-EndFunction -CommandName $MyInvocation.MyCommand.Name -Duration ($endFunction -$startFunction)

    }
    Process
    {

        try
        {
            $body = @{
                query = $Query
            }

            if ($null -ne $Variables)
            {
                $body["variables"] = $Variables
            }

            $jsonBody = $body | ConvertTo-Json -Depth 5

            Trace-Message -Message "Body: $jsonBody" -CommandName $MyInvocation.MyCommand.Name

            $result = Invoke-RestMethod -Uri $Uri -Method Post -Body $jsonBody -ContentType "application/json" -ErrorAction stop

            if ($null -ne $result.errors -and $result.errors.length -gt 0)
            {
                $errorMessage = ""
                foreach($resultErr in $result.errors)
                {
                    $errorMessage += "{0} - {1} : {2}`n" -f ($resultErr.path -join ","), $resultErr.message, $resultErr.extensions.message
                }
                throw $errorMessage
            }
            $result

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