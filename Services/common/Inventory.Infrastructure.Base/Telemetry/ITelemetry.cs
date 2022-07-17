using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Base.Telemetry
{
    /// <summary>
    /// Telemetry helpers
    /// </summary>
    public interface ITelemetry
    {

        Activity Current { get; }

        void SetOtelError(string error, bool log = false);

        void SetOtelError(Exception ex);

        void SetOtelWarning(string error);

        string GetTraceId();

        ActivitySource AppSource { get; }

    }
}
