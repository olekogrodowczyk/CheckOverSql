using Application.Groups.Queries.GetAllSolvingsAssignedToUserToDo;
using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Solvings.Queries.GetAllSolvingsAssignedToUserByStatus
{
    public class GetAllSolvingsAssignedToUserByStatusQueryValidator : AbstractValidator<GetAllSolvingsAssignedToUserByStatusQuery>
    {
        public GetAllSolvingsAssignedToUserByStatusQueryValidator()
        {
            RuleFor(x => x.Status)
                .Must(checkStatus)
                .WithMessage($"Valid status hasn't been specified" +
                $", available statuses: {string.Join(',', Enum.GetNames(typeof(SolvingStatusEnum)))}");
        }

        private bool checkStatus(string value)
        {
            SolvingStatusEnum solvingStatusEnum;
            if (Enum.TryParse(value, out solvingStatusEnum)) { return true; }
            return false;
        }
    }
}
