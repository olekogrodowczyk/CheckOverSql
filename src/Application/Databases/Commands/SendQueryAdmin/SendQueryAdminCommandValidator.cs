using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Databases.Commands.SendQueryAdmin
{
    public class SendQueryAdminCommandValidator : AbstractValidator<SendQueryAdminCommand>
    {
        private readonly IDatabaseRepository _databaseRepository;

        private string[] getDatabasesNames()
        {
            var databases = _databaseRepository.GetAllAsync().Result;
            return databases.Select(x => x.Name.ToLower()).ToArray();
        }
        public SendQueryAdminCommandValidator(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
            string[] databasesNames = getDatabasesNames();
            RuleFor(x => x.Query)
                .NotEmpty().WithMessage("Zapytanie nie może być puste")
                .MaximumLength(200).WithMessage("Zapytanie może liczyć maksymalnie 200 znaków");
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
