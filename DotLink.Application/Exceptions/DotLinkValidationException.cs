using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.Exceptions
{
    public class DotLinkValidationException : Exception
    {
        public List<FluentValidation.Results.ValidationFailure?> ValidationErrors { get; init; }
        public DotLinkValidationException(List<FluentValidation.Results.ValidationFailure?> failures)
            : base("Validation error occured") { 
            ValidationErrors = failures;
        }
    }
}
