using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.CreateExerciseDto
{
    public class CreateExerciseDtoValidator : AbstractValidator<CreateExerciseDto>
    {
        private readonly IDatabaseRepository _databaseRepository;

        private string[] getDatabasesNames()
        {
            var databases = _databaseRepository.GetAll().Result;
            return databases.Select(x => x.Name.ToLower()).ToArray();
        }

        public CreateExerciseDtoValidator(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;

            string[] databasesNames = getDatabasesNames();

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Nie podano tytułu zadania")
                .MinimumLength(5).WithMessage("Minimalna długość tytułu to 5")
                .MaximumLength(200).WithMessage("Maksymalna długość tytułu to 200");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Nie podano opisu zadania")
                .MinimumLength(10).WithMessage("Minimalna długość opisu to 10")
                .MaximumLength(3000).WithMessage("Maksymalna długość opisu to 3000");

            RuleFor(x => x.MaxPoints)
                .NotEmpty().WithMessage("Nie podano maksymalnej ilości punktów zadania")
                .GreaterThanOrEqualTo(1).WithMessage("Minimalna liczba maksymalnej ilości punktów zadania to 1")
                .LessThanOrEqualTo(100).WithMessage("Maksymalna liczba maksymalnej ilości punktów zadania to 100");

            RuleFor(x => x.Answer)
                .MinimumLength(10).WithMessage("Minimalna długość odpowiedzi do zadania to 5")
                .MaximumLength(200).WithMessage("Maksymalna długość odpowiedzi do zadania to 500");


            RuleFor(x => x.Database)
                .NotEmpty().WithMessage("Nie wybrano bazy danych")
                .MaximumLength(25).WithMessage("Wybrana baza danych nie może mieć więcej niż 25 znaków")
                .Custom((value, context) =>
                {
                    if (!databasesNames.Contains(value.ToLower()))
                    {
                        context.AddFailure("Database", $"Podana baza danych nie istnieje, dostępne bazy danych to " +
                            $"[{string.Join(",", databasesNames)}]");
                    }
                });          
        }
    }
}
