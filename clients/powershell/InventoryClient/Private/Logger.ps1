function Trace-Message
{
    [CmdletBinding()]
    param(
        [string]
        $Message,

        [String]
        $CommandName
    )

    write-verbose ("{0} - {1} : {2}" -f (Get-Date), $CommandName, $Message)

}

function Trace-StartFunction
{
    [CmdletBinding()]
    param(
        [String]
        $CommandName
    )

    write-verbose ("{0} - {1} : Start" -f (Get-Date), $CommandName)

}

function Trace-EndFunction
{
    [CmdletBinding()]
    param(
        [String]
        $CommandName,

        [System.TimeSpan]
        $Duration
    )

    write-verbose ("{0} - {1} : End => Duration {2} ms" -f (Get-Date), $CommandName, $Duration.TotalMilliseconds)

}

