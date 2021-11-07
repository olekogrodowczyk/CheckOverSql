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
    public class ExerciseRepository : EfRepository<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(ApplicationDbContext context, ILogger<ExerciseRepository> logger) : base(context, logger)
        {
        }


    }
}
