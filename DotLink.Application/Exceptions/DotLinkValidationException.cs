using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.Exceptions
{
    public class DotLinkValidationException : Exception
    {
        private readonly List<FluentValidation.Results.ValidationFailure?> _validationErrors;
        public DotLinkValidationException(List<FluentValidation.Results.ValidationFailure?> failures)
            : base("Validation error occured") { 
            _validationErrors = failures;
        }
    }
}
