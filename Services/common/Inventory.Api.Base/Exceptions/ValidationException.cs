using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api.Base.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new ValidationFailure[0];
        }

        public ValidationException(ValidationFailure[] failures)
            : this()
        {
            Errors = failures;
        }

        public ValidationException(string message)
        : base(message)
        {
            Errors = new ValidationFailure[0];
        }

        public ValidationFailure[] Errors { get; }
    }

}
