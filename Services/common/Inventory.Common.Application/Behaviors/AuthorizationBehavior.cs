using FluentValidation;
using FluentValidation.Results;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Users;
using Inventory.Common.Infrastructure.Telemetry;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Behaviors
{
    /// <summary>
    /// Authorization marker interface for Fluent validation
    /// </summary>
    public interface IAuthorizationValidator { }

    /// <summary>
    /// Authorization validator wrapper
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public class AuthorizationValidator<TRequest>
    : AbstractValidator<TRequest>, IAuthorizationValidator
    { }

    /// <summary>
    /// Authorization behaviour for MediatR pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class AuthorizationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentUser _currentUserService;
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<AuthorizationBehavior<TRequest, TResponse>> _logger;
        private readonly ITelemetry _telemetry;

        public AuthorizationBehavior(
            ICurrentUser currentUserService,
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<AuthorizationBehavior<TRequest, TResponse>> logger,
            ITelemetry telemetry)
        {
            _currentUserService = currentUserService;
            _validators = validators;
            _logger = logger;
            _telemetry = telemetry;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {

            var authorizeAttributes = request.GetType().GetCustomAttributes(typeof(AuthorizeAttribute), true);

            if (authorizeAttributes.Any())
            {

                var activity = GetActivity(request);

                try
                {
                    activity?.Start();

                    // Must be authenticated user
                    if (!_currentUserService.Exist)
                        return HandleUnAuthorised(null);

                    // Role-based authorization
                    var authorizeAttributesWithRoles = authorizeAttributes.Where(
                        a => !string.IsNullOrWhiteSpace((a as AuthorizeAttribute).Roles)
                    );

                    if (authorizeAttributesWithRoles.Any())
                    {

                        foreach (var roles in authorizeAttributesWithRoles.Select(a => (a as AuthorizeAttribute).Roles.Split(',')))
                        {
                            var authorized = false;

                            foreach (var role in roles)
                            {

                                if (_currentUserService.HasRole(role.Trim()))
                                {
                                    authorized = true;
                                    break;
                                }
                            }

                            // Must be a member of at least one role in roles
                            if (!authorized)
                            {
                                return HandleUnAuthorised("Role authorization failure");
                            }
                        }
                    }

                    // Policy-based authorization
                    var authorizeAttributesWithPolicies = authorizeAttributes.Where(
                        a => !string.IsNullOrWhiteSpace((a as AuthorizeAttribute).Policy)
                    );

                    if (authorizeAttributesWithPolicies.Any())
                    {
                        foreach (var policy in authorizeAttributesWithPolicies.Select(a => (a as AuthorizeAttribute).Policy))
                        {
                            if (!_currentUserService.HasRole(policy.Trim()))
                            {
                                return HandleUnAuthorised($"Policy: {policy} authorization failure");
                            }
                        }
                    }

                    // Inner command validator autorization checks
                    var authorizeAttributesWithInnerPolicies = authorizeAttributes.Where(
                        a => (a as AuthorizeAttribute).InnerPolicy == true
                    );

                    if (authorizeAttributesWithInnerPolicies.Any())
                    {

                        IValidator<TRequest>[] authValidators = _validators.Where(
                            v => v is IAuthorizationValidator).ToArray();

                        ValidationFailure[] authorization_validator_failures =
                            await CommandAuthValidationFailuresAsync(request, authValidators);

                        if (authorization_validator_failures.Any())
                            return HandleUnAuthorised(authorization_validator_failures);

                    }

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

        private static TResponse HandleUnAuthorised(object error_obj)
        {

            // In case it is Mutation Response Payload = handled as payload error union
            if (Inventory.Common.Application.Common.IsSubclassOfRawGeneric(
                typeof(BasePayload<,>),
                typeof(TResponse))
            )
            {
                var payload = (IPayload)Activator.CreateInstance<TResponse>();

                if (error_obj is ValidationFailure[])
                {
                    foreach (var item in error_obj as ValidationFailure[])
                    {
                        payload.AddError(new UnAuthorisedError(item.CustomState, item.ErrorMessage));
                    }
                }
                else if (error_obj is string)
                {
                    payload.AddError(new UnAuthorisedError(error_obj as string));
                }
                else
                {
                    payload.AddError(new UnAuthorisedError());
                }

                return (TResponse)payload;
            }
            else
            {
                // In case it is query response = handled by global filter
                if (error_obj is ValidationFailure[])
                {
                    throw new UnAuthorisedException(error_obj as ValidationFailure[]);
                }
                else if (error_obj is string)
                {
                    throw new UnAuthorisedException(error_obj as string);
                }
                else
                {
                    throw new UnAuthorisedException();
                }
            }
        }

        private static async Task<ValidationFailure[]> CommandAuthValidationFailuresAsync(
            TRequest request,
            IEnumerable<IValidator<TRequest>> authValidators)
        {

            var validateTasks = authValidators
                .Select(v => v.ValidateAsync(request));

            var validateResults = await Task.WhenAll(validateTasks);

            var validationFailures = validateResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToArray();

            return validationFailures == null ?
                new ValidationFailure[0] : validationFailures;
        }

        private Activity GetActivity(TRequest request)
        {
            return _telemetry.AppSource.StartActivity(
                    String.Format(
                        "AuthorizationBehaviour: Request<{0}>",
                        request.GetType().FullName),
                        ActivityKind.Server);
        }

    }
}
