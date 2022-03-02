using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Databases.Queries.GetQueryHistory
{
    public class GetQueryHistoryQueryValidator : AbstractValidator<GetQueryHistoryQuery>
    { 
        public GetQueryHistoryQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number has to be at least or equal 1");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size has to be at least or equal 1");
        }
    }
}
