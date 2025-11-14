using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Search
{
    public class CompositeSearchQueryValidator : AbstractValidator<CompositeSearchQuery>
    {
        public CompositeSearchQueryValidator()
        {
            RuleFor(x => x.SearchTerm)
                .NotEmpty().WithMessage("Search term must not be empty.")
                .MaximumLength(100).WithMessage("Search term has to have maximum length of 100 characters.");
        }
    }
}
