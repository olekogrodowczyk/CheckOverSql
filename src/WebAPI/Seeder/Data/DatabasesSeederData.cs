using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Seeders
{
    public static class DatabasesSeederData
    {
        //The real data needs be specified by user
        public static IEnumerable<Database> GetDatabases()
        {
            return new List<Database>()
            {
                new Database()
                {
                    ConnectionString=
                    new ConnectionString("NorthwindSimple Server", "NorthwindSimple database name", "User", "Password"),
                    Name = "NorthwindSimple",
                }
            };
        }
    }
}
