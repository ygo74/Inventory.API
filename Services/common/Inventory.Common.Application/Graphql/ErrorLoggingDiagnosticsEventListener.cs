using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution.Processing;
using HotChocolate.Execution;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;

namespace Inventory.Common.Application.Graphql
{
    // https://chillicream.com/docs/hotchocolate/v12/server/instrumentation
    public class ErrorLoggingDiagnosticsEventListener : ExecutionDiagnosticEventListener
    {
        private readonly ILogger<ErrorLoggingDiagnosticsEventListener> log;

        public ErrorLoggingDiagnosticsEventListener(
            ILogger<ErrorLoggingDiagnosticsEventListener> log)
        {
            this.log = log;
        }

        public override void ResolverError(
            IMiddlewareContext context,
            IError error)
        {
            log.LogError(error.Exception, error.Message);
        }

        public override void TaskError(
            IExecutionTask task,
            IError error)
        {
            log.LogError(error.Exception, error.Message);
        }

        public override void RequestError(
            IRequestContext context,
            Exception exception)
        {
            log.LogError(exception, "RequestError");
        }

        public override void SubscriptionEventError(
            SubscriptionEventContext context,
            Exception exception)
        {
            log.LogError(exception, "SubscriptionEventError");
        }

        public override void SubscriptionTransportError(
            ISubscription subscription,
            Exception exception)
        {
            log.LogError(exception, "SubscriptionTransportError");
        }

        // this is invoked at the start of the `ExecuteRequest` operation
        public override IDisposable ExecuteRequest(IRequestContext context)
        {
            var start = DateTime.UtcNow;

            return new RequestScope(start, log);
        }

        public override void SyntaxError(IRequestContext context, IError error)
        {
            log.LogError(error.Message);
        }

        public override void ValidationErrors(IRequestContext context, IReadOnlyList<IError> errors)
        {
            foreach(var error in errors)
            {
                log.LogError(error.Message);
            }
        }
    }

    public class RequestScope : IDisposable
    {
        private readonly ILogger _logger;
        private readonly DateTime _start;

        public RequestScope(DateTime start, ILogger logger)
        {
            _start = start;
            _logger = logger;
        }

        // this is invoked at the end of the `ExecuteRequest` operation
        public void Dispose()
        {
            var end = DateTime.UtcNow;
            var elapsed = end - _start;

            _logger.LogInformation("Request finished after {Ticks} ticks",
                elapsed.Ticks);
        }
    }
}
