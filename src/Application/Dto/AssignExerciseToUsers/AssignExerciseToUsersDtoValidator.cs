using Application.Dto.AssignExerciseToUsersTo;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.AssignExerciseToUsers
{
    public class AssignExerciseToUsersDtoValidator : AbstractValidator<AssignExerciseToUsersDto>
    {
        public AssignExerciseToUsersDtoValidator()
        {
            RuleFor(x => x.DeadLine)
                .GreaterThanOrEqualTo(DateTime.UtcNow.AddHours(1))
                .WithMessage("Defined date isn't valid, deadline can be only set one hour later from now");
        }
    }
}
