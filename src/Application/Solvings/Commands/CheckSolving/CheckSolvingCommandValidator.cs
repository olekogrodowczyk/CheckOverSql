using Application.Common.Exceptions;
using Application.Solvings.Commands.CheckExercise;
using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Solvings.Commands.CheckSolving
{
    public class CheckSolvingCommandValidator : AbstractValidator<CheckSolvingCommand>
    {
        private readonly ISolvingRepository _solvingRepository;

        public CheckSolvingCommandValidator(ISolvingRepository solvingRepository)
        {
            _solvingRepository = solvingRepository;

            RuleFor(x => x.Points)
                .NotEmpty().WithMessage("Defined points cannot be empty")
                .GreaterThanOrEqualTo(0).WithMessage("Points value cannot be negative")
                .MustAsync(async (model, points, cancellationToken) =>
                { return await validatePoints(points, model.SolvingId, cancellationToken); })
                .WithMessage("Specified points value is invalid");

            RuleFor(x => x.SolvingId)
                .NotEmpty().WithMessage("Defined solving identifier cannot be empty");
        }

        private async Task<bool> validatePoints(int points, int solvingId, CancellationToken cancellationToken)
        {
            var solving = await _solvingRepository.GetByIdAsync(solvingId);
            if (solving is null) { throw new NotFoundException(nameof(solving), solvingId); }
            if (points > solving.MaxPoints) { return false; }
            return true;
        }
    }
}
