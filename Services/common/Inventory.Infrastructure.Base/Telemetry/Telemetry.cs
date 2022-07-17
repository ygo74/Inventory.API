using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Base.Telemetry
{
    /// <summary>Telemetry helpers</summary>
    public class Telemetry : ITelemetry
    {

        private readonly IOptions<TelemetryOptions> _options;

        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// Main constructor of TelemetryProvider
        /// </summary>
        public Telemetry(IOptions<TelemetryOptions> options, IHttpContextAccessor accessor)
        {

            _options = options;

            _accessor = accessor;

            if (_options != null)
            {
                AppSource = new(_options.Value.SourceName);
            }
            else
            {
                AppSource = new ActivitySource("default");
            }

        }

        public Activity Current { get { return Activity.Current; } }

        public ActivitySource AppSource { get; private set; }

        public void SetOtelError(string error, bool log = false)
        {

            var current = Activity.Current;
            current?.SetTag("otel.status_code", "ERROR");

            if (!string.IsNullOrWhiteSpace(error))
            {

                current?.SetTag("otel.status_description", error);

                //if (log)
                //    Log.Error(error);
            }
        }

        public void SetOtelError(Exception ex)
        {

            if (ex == null)
                return;

            if (!ex.Data.Contains("command_failed"))
            {

                SetOtelError(ex.ToString(), true);
            }
        }

        public void SetOtelWarning(string message)
        {

            var current = Activity.Current;

            current?.SetTag("otel.status_code", "WARNING");

            if (!string.IsNullOrWhiteSpace(message))
            {
                current?.SetTag("otel.status_description", message);
            }
        }

        public string GetTraceId()
        {
            return Activity.Current?.TraceId.ToString() ?? _accessor?.HttpContext?.TraceIdentifier;
        }
    }
}
