using Application.Exercises.Commands.CreateExercise;
using Application.Interfaces;
using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Dto.CreateExerciseDto
{
    public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IUserContextService _userContextService;
        private string[] databaseNames; 

        private async Task<bool> containsDatabaseName(string value, CancellationToken cancellationToken)
        {
            var databases = await _databaseRepository.GetAllAsync();
            databaseNames = databases.Select(x => x.Name.ToLower()).ToArray();
            return databaseNames.Contains(value);
        }

        public CreateExerciseCommandValidator(IDatabaseRepository databaseRepository, IUserContextService userContextService)
        {
            _databaseRepository = databaseRepository;
            _userContextService = userContextService;
            
            int userId = _userContextService.GetUserId ?? 0;


            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title of the exercise hasn't been defined")
                .MinimumLength(5).WithMessage("Minimum length of title is 5")
                .MaximumLength(200).WithMessage("Maximum length of the title is 200");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description of the exercise hasn't been defined")
                .MinimumLength(10).WithMessage("Minimum length of description is 10")
                .MaximumLength(3000).WithMessage("Maximum length of description is 3000");

            RuleFor(x => x.MaxPoints)
                .NotEmpty().WithMessage("Max points hasn't been specified")
                .GreaterThanOrEqualTo(1).WithMessage("Minimum value of max points is 1")
                .LessThanOrEqualTo(100).WithMessage("Maximum value of max points is 100");

            RuleFor(x => x.ValidAnswer)
                .MinimumLength(10).WithMessage("Minimum length of valid answer is 5")
                .MaximumLength(500).WithMessage("Maximum length of valid answer is 500");

            RuleFor(x => x.Database)
                .NotEmpty().WithMessage("Database hasn't been chosen")
                .MaximumLength(25).WithMessage("Max length of chosen database is 25")
                .MustAsync(containsDatabaseName)
                .WithMessage($"Defined database doesn't exist, available databases: [{string.Join(',', databaseNames)}]");

            RuleFor(x => x.IsPrivate)
                .NotEmpty().WithMessage("Exercise visibility hasn't been defined");
        }
    }
}
