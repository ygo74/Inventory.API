function New-InventoryCredential
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(ParameterSetName="Pipeline", Position=0, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [PsObject]
        $InputObject,

        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Name,

        [Parameter(ParameterSetName="Default", Position=1, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Description
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

        if ($PsCmdlet.ParameterSetName -eq "Pipeline")
        {
            $Name          = $InputObject.Name
            $Description   = $InputObject.Description
        }

        # Display input properties
        Trace-Message -Message ("Credential Name : '{0}'" -f $Name) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Credential Description : '{0}'" -f $Description) -CommandName $MyInvocation.MyCommand.Name

        # Assert Mandatory variables
        if ([string]::IsNullOrWhiteSpace($Name)) {throw "Name is mandatory"}

        # Create payload input
        $graphqlInput = @{
            name = $Name
        }
        if (![string]::IsNullOrWhiteSpace($Description)) {$graphqlInput[ "description"] = $Description}

        $Variables = @{
            input = $graphqlInput
        }

        $command = $script:CreateCredentialMutation + $script:CredentialDtoFragment + $script:ErrorsFragment

        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.createCredential.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.createCredential.errors)
        }
        $result.createCredential.data

    }
}

function Get-InventoryCredential
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(ParameterSetName="ById", Position=0, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [int]
        $Id,

        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Name

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

        $Variables = @{}
        switch ($PsCmdlet.ParameterSetName)
        {
            "ById"
            {
                Trace-Message -Message ("Get Credential By Id : '{0}'" -f $Id) -CommandName $MyInvocation.MyCommand.Name
                $Variables[ "id" ] = $Id
                $command = $script:GetCredentialByIdQuery + $script:CredentialDtoFragment + $script:ErrorsFragment
            }
            "Default"
            {
                Trace-Message -Message ("Get Credential By Name : '{0}'" -f $Name) -CommandName $MyInvocation.MyCommand.Name
                $Variables[ "name" ] = $Name

                $command = $script:GetCredentialByNameQuery + $script:CredentialDtoFragment + $script:ErrorsFragment
            }
            default {
                throw "Invalid ParameterSetName"
            }
        }

        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.credential.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.credential.errors)
        }
        $result.credential.data

    }
}

function Find-InventoryCredential
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(Position=0, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [String]
        $Name,

        [Parameter(Position=1, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [Int]
        $Skip,

        [Parameter(Position=2, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [Int]
        $Take

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
        Trace-Message -Message ("Credential Name : {0}" -f $Name) -CommandName $MyInvocation.MyCommand.Name

        # Filter
        $query = @{}
        if (![string]::IsNullOrWhiteSpace($Name)) {$query[ "name"] = $Name}

        $Variables = @{
            query = $query
        }

        # Pagination
        if ($Skip -gt 0) { $Variables[ "skip" ] = $Skip }
        if ($Take -gt 0) { $Variables[ "take" ] = $Take }

        # query
        $Query = $script:FindCredentialQuery + $script:CollectionSegmentInfo +$script:CredentialDtoFragment
        $result = Invoke-InternalGraphql -Query $Query -Variables $Variables -uri $global:ConfigurationUri

        $cmdletResult = $result.credentials.pageInfo
        $cmdletResult | Add-Member -MemberType NoteProperty -Name totalCount -Value $result.credentials.totalCount
        $cmdletResult | Add-Member -MemberType NoteProperty -Name items -Value $result.credentials.items
        return $cmdletResult

    }
}

