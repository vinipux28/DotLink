using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace DotLink.Application.Commands.PostCommands
{
    public class CastVoteCommandValidator : AbstractValidator<CastVoteCommand>
    {
        public CastVoteCommandValidator() { 
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId is required.");
        }
    }
}
