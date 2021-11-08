using Domain.Entities;
using Domain.Enums;
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
    public class InvitationRepository : EfRepository<Invitation>, IInvitationRepository
    {
        public InvitationRepository(ApplicationDbContext context, ILogger<EfRepository<Invitation>> logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<Invitation>> GetInvitationsWithAllIncludes()
        {
            var invitationsWithIncludes = _context.Invitations
                .Include(x => x.Receiver)
                .Include(x => x.GroupRole)
                .Include(x => x.Sender)
                .Include(x => x.Group);

            var result = await invitationsWithIncludes.ToListAsync();

            return result;
        }

    }
}
