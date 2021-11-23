using Application.Dto.AssignExerciseToUsersTo;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.AssignExerciseToUsers
{
    public class AssignExerciseToUsersCommandValidator : AbstractValidator<AssignExerciseToUsersCommand>
    {
        public AssignExerciseToUsersCommandValidator()
        {
            RuleFor(x => x.DeadLine)
                .GreaterThanOrEqualTo(DateTime.UtcNow.AddHours(1))
                .WithMessage("Defined date is invalid, deadline can be only set one hour later from now");
        }
    }
}
