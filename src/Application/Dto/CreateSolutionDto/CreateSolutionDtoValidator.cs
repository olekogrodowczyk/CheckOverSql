using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Dto.CreateSolutionDto
{
    public class CreateSolutionDtoValidator : AbstractValidator<CreateSolutionDto>
    {
        private readonly ISolvingRepository _solvingRepository;

        public CreateSolutionDtoValidator(ISolvingRepository solvingRepository)
        {
            _solvingRepository = solvingRepository;

            RuleFor(x => x.Query)
                .NotEmpty().WithMessage("Query cannot be empty")
                .MaximumLength(500).WithMessage("Maximum length of query is 500");          
        }
    }
}
