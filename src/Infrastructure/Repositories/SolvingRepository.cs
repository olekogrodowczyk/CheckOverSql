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
    public class SolvingRepository : EfRepository<Solving>, ISolvingRepository
    {
        public SolvingRepository(ApplicationDbContext context, ILogger<EfRepository<Solving>> logger) : base(context, logger)
        {
        }
    }
}
