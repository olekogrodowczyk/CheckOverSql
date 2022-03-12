using Application.Solvings.Commands.CheckExercise;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Solvings.Commands.CheckSolving
{
    public class CheckSolvingCommandValidator : AbstractValidator<CheckSolvingCommand>
    {
        public CheckSolvingCommandValidator()
        {
            RuleFor(x => x.Points)
                .NotEmpty().WithMessage("Defined points cannot be empty");

            RuleFor(x => x.SolvingId)
                .NotEmpty().WithMessage("Defined solving identifier cannot be empty");
        }
    }
}
