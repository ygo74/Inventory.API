using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Users
{
    public class UnAuthorisedException : Exception
    {
        public UnAuthorisedException()
            : base("One or more authorization failures have occurred.")
        {
            Errors = new ValidationFailure[0];
        }

        public UnAuthorisedException(ValidationFailure[] failures)
            : this()
        {
            Errors = failures;
        }

        public UnAuthorisedException(string message)
        : base(message)
        {
            Errors = new ValidationFailure[0];
        }

        public ValidationFailure[] Errors { get; }

    }
}
