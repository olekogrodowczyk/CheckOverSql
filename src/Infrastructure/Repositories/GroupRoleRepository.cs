using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GroupRoleRepository : EfRepository<GroupRole>, IGroupRoleRepository
    {
        public GroupRoleRepository(ApplicationDbContext context, ILogger<GroupRoleRepository> logger) : base(context, logger)
        {
        }

        public async Task<GroupRole> GetByName(string name)
        {
           var groupRole =  await _context.GroupRoles.FirstOrDefaultAsync(x => x.Name == name);
           if (groupRole == null) { throw new NotFoundException(nameof(groupRole), groupRole.Id); }
           return groupRole;
        }
    }
}
