using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GroupRoleRepository : EfRepository<GroupRole>, IGroupRoleRepository
    {
        public GroupRoleRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<GroupRole> GetByName(string name)
        {
           var result =  await _context.GroupRoles.FirstOrDefaultAsync(x => x.Name == name);
           if (result == null) { throw new NotFoundException($"Result is not found with name:{name}"); }
           return result;
        }
    }
}
