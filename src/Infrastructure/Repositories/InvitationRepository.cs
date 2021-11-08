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
    public class InvitationRepository : EfRepository<Invitation>, IInvitationRepository
    {
        public InvitationRepository(ApplicationDbContext context, ILogger<EfRepository<Invitation>> logger) : base(context, logger)
        {
        }
    }
}
