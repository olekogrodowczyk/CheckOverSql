using Application.Dto.CreateInvitationDto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IInvitationService
    {
        Task CheckIfInvitationAlreadyExists(string email, string role, int groupId);
        Task CheckIfUserIsAlreadyInGroup(string email, string role, int groupId);
        Task<int> CreateInvitation(CreateInvitationDto model, int groupId);
    }
}
