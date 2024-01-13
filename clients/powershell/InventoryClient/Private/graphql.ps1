
$script:PaginationInfo=@"
fragment pageInfo on PageInfo
{
  hasNextPage
  hasPreviousPage
  startCursor
  endCursor
}
"@

$script:CollectionSegmentInfo=@"
fragment pageInfo on CollectionSegmentInfo
{
  hasNextPage
  hasPreviousPage
}
"@


$script:ErrorsFragment=@"
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
            Trace-Message -Message "Query: =>" -CommandName $MyInvocation.MyCommand.Name
            Trace-Message -Message "$Query" -CommandName $MyInvocation.MyCommand.Name

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
                    $errorMessage += $resultErr.extensions.stackTrace
                }
                throw $errorMessage
            }

            $result.data

        }
        catch [System.Net.WebException]
        {
            if ($null -eq $_.Exception.Response)
            {
                throw $_
            }

            Trace-Message -Message "ERROR" -CommandName $MyInvocation.MyCommand.Name
            $errorMessage = $_.exception.Response | ConvertTo-Json
            Trace-Message -Message $errorMessage -CommandName $MyInvocation.MyCommand.Name
            $respStream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($respStream)
            $respBody = $reader.ReadToEnd() | ConvertFrom-Json
            if ($null -ne $respBody -and $respBody.errors.length -gt 0)
            {
                $errorMessage = ""
                foreach($resultErr in $respBody.errors)
                {
                    $errorMessage += "{0} - {1} : {2}`n" -f ($resultErr.path -join ","), $resultErr.message, $resultErr.extensions.message
                }
                Trace-Message -Message $errorMessage -CommandName $MyInvocation.MyCommand.Name
                throw $errorMessage
            }

            throw $_
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