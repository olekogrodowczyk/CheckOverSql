using Application.Dto.CreateInvitationDto;
using Application.ViewModels;
using AutoMapper;
using Domain.Enums;
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
        Task CheckIfSenderIsInTheGroup(int groupId);
        Task CheckIfUserIsAlreadyInGroup(string email, string role, int groupId);
        Task<int> CreateInvitation(CreateInvitationDto model, int groupId);
        Task<IEnumerable<GetInvitationVm>> GetAllUserInvitations();
        Task<IEnumerable<GetInvitationVm>> GetAllUserReceivedInvitations();
        Task<IEnumerable<GetInvitationVm>> GetAllUserSentInvitations();
    }
}
