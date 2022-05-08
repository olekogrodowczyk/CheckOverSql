using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Solutions.Commands.CreateSolution
{
    public class CreateSolutionCommandValidator : AbstractValidator<CreateSolutionCommand>
    {
        private readonly ISolvingRepository _solvingRepository;

        public CreateSolutionCommandValidator(ISolvingRepository solvingRepository)
        {
            _solvingRepository = solvingRepository;

            RuleFor(x => x.Query)
                .NotEmpty().WithMessage("Query cannot be empty");
        }
    }
}
