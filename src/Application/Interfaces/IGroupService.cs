﻿using Application.Responses;
using Application.Dto.CreateGroupVm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Solvings;

namespace Application.Interfaces
{
    public interface IGroupService
    {
        Task<int> CreateGroup(CreateGroupDto model);
        Task DeleteGroup(int groupId);
        Task<IEnumerable<GetAssignmentVm>> GetAllAssignmentsInGroup(int groupId);
        Task<IEnumerable<GetGroupVm>> GetUserGroups();
    }
}
