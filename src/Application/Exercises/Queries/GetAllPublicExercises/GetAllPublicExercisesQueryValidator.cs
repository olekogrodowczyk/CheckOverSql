using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exercises.Queries.GetAllPublicExercises
{
    public class GetAllPublicExercisesQueryValidator : AbstractValidator<GetAllPublicExercisesQuery>
    {
        public GetAllPublicExercisesQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number has to best at least or equal 1");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size has to best at least or equal 1");
        }
    }
}
