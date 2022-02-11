using Application.Databases.Queries.GetQueryValueAdmin;
using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Databases.Queries.GetQueryValue
{
    public class GetQueryValueQueryValidator : AbstractValidator<GetQueryValueQuery>
    {
        private readonly IDatabaseRepository _databaseRepository;

        public GetQueryValueQueryValidator(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public GetQueryValueQueryValidator()
        {
            RuleFor(x => x.DatabaseName)
                .NotEmpty().WithMessage("Database haven't been specified")
                .MustAsync(DatabaseExists).WithMessage("Defined database doesn't exist");

            RuleFor(x => x.Query)
                .NotEmpty().WithMessage("Query hasn't been specified");
        }

        public async Task<bool> DatabaseExists(string value, CancellationToken cancellationToken)
        {
            return await _databaseRepository.AnyAsync(x=>x.Name == value);
        }
    }
}
