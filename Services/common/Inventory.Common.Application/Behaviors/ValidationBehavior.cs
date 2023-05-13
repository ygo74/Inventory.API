using FluentValidation;
using FluentValidation.Results;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Exceptions;
using Inventory.Common.Application.Users;
using Inventory.Common.Infrastructure.Telemetry;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Inventory.Common.Application.Exceptions.ValidationException;

namespace Inventory.Common.Application.Behaviors
{
    //public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    //{
    //    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    //    private readonly IEnumerable<IValidator<TRequest>> _validators;

    //    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    //    {
    //        _validators = validators;
    //        _logger = logger;
    //    }

    //    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    //    {
    //        var typeName = request.GetType().Name;

    //        _logger.LogInformation("----- Validating command {CommandType}", typeName);

    //        var failures = _validators
    //            .Select(v => v.Validate(request))
    //            .SelectMany(result => result.Errors)
    //            .Where(error => error != null)
    //            .ToList();

    //        if (failures.Any())
    //        {
    //            _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

    //            //throw new Exception(
    //            //    $"Command Validation Errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures));

    //            throw new ValidationException($"Validation exception for type {typeof(TRequest).Name}", failures);
    //        }

    //        return await next();
    //    }
    //}

    /// <summary>
    /// Validation bhaviour for MediatR pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ValidationBehavior<TRequest, TResponse>
     : IPipelineBehavior<TRequest, TResponse> where TResponse: IPayload
    {
        private readonly ICurrentUser _currentUser;
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
        private readonly ITelemetry _telemetry;

        public ValidationBehavior(
            ICurrentUser currentUser,
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehavior<TRequest, TResponse>> logger,
            ITelemetry telemetry)
        {
            _currentUser = currentUser;
            _validators = validators;
            _logger = logger;
            _telemetry = telemetry;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {

            if (_validators.Any())
            {
                var activity = GetActivity(request);

                try
                {
                    activity?.Start();

                    var context = GetValidationCtx(request);

                    var failures = await Validate<TRequest>(context, cancellationToken);

                    if (failures.Count != 0)
                        return HandleValidationErrors(failures);
                }
                catch (Exception ex)
                {
                    _telemetry.SetOtelError(ex);

                    throw;
                }
                finally
                {
                    activity?.Stop();
                    activity?.Dispose();
                }
            }

            // Continue in pipe
            return await next();
        }

        private async Task<List<ValidationFailure>> Validate<T>(
            ValidationContext<T> ctx, CancellationToken ct)
        {
            var validationResults = await Task.WhenAll(
            _validators.Where(v => !(v is IAuthorizationValidator))
            .Select(v => v.ValidateAsync(ctx, ct)));

            return validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();
        }

        private ValidationContext<TRequest> GetValidationCtx(TRequest request)
        {
            return new ValidationContext<TRequest>(request);
        }

        private static TResponse HandleValidationErrors(List<ValidationFailure> error_obj)
        {

            // In case it is Mutation Response Payload = handled as payload error union
            if (Common.IsSubclassOfRawGeneric(
                typeof(Payload<>),
                typeof(TResponse))
            )
            {
                var payload = ((IPayload)Activator.CreateInstance<TResponse>());

                foreach (var item in error_obj)
                {
                    payload.AddError(
                        new ValidationError(
                            item.PropertyName,
                            item.ErrorMessage)
                    );
                }

                return (TResponse)payload;
            }
            else
            {

                if (error_obj != null)
                {

                    var first_item = error_obj.First();

                    if (first_item != null)
                    {
                        throw new ValidationException(
                            string.Format(
                                "Field: {0} - {1}",
                                first_item.PropertyName,
                                first_item.ErrorMessage
                            )
                        );
                    }

                }
                throw new ValidationException("Validation error appear");

            }
        }

        private Activity GetActivity(TRequest request)
        {
            return _telemetry.AppSource.StartActivity(
                    String.Format(
                        "ValidationBehaviour: Request<{0}>",
                        request.GetType().FullName),
                        ActivityKind.Server);
        }
    }


}
