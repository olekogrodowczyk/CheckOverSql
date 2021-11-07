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
    public class ComparisonRepository : EfRepository<Comparison>, IComparisonRepository
    {
        public ComparisonRepository(ApplicationDbContext context, ILogger<ComparisonRepository> logger) : base(context, logger)
        {

        }
    }
}
