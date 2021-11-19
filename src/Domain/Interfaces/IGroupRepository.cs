﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<IEnumerable<Assignment>> GetAllAssignmentsInGroup(int groupId);
        Task<IEnumerable<Group>> GetUserGroups(int userId);
    }
}
