using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GroupRepository : EfRepository<Group>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext context, ILogger<GroupRepository> logger) : base(context, logger)
        {
        }

    }
}
