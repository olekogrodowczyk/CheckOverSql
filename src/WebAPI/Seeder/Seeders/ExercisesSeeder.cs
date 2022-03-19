using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Seeders;

namespace WebAPI.Seeder.Seeders
{
    public class ExercisesSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;

        public ExercisesSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            int? superUserId = _context.Users.FirstOrDefault(x => x.Email == "superuser@gmail.com")?.Id;
            if (superUserId is null) { return; }
            int? nortwindSimpleDatabaseId = _context.Databases.SingleOrDefault(x => x.Name == "NorthwindSimple")?.Id;
            if (nortwindSimpleDatabaseId is null) { return; }

            var exercises = ExercisesSeederData.GetNorthwindSimplePublicExercises((int)superUserId, (int)nortwindSimpleDatabaseId);
            var publicExercisesCount = exercises.Count();
            int countOfExercisesInDatabase = _context.Exercises.Where(x => !x.IsPrivate && x.CreatorId == superUserId).Count();

            if (countOfExercisesInDatabase < publicExercisesCount)
            {
                int countOfRemainingExercises = publicExercisesCount - countOfExercisesInDatabase;
                _context.Exercises.AddRange(exercises.TakeLast(countOfRemainingExercises));
                _context.SaveChanges();
            }
        }
    }
}
