using Application.Responses;
using Application.Dto.CreateGroupVm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.GetGroupDto;

namespace Application.Interfaces
{
    public interface IGroupService
    {
        Task<int> CreateGroupAsync(CreateGroupDto model);
        Task<IEnumerable<GetGroupDto>> GetUserGroups();
    }
}
