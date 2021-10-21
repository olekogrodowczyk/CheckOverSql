﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDatabaseRepository : IRepository<Database>
    {
        Task<string> GetAdminDatabaseConnectionStringByName(string name);
        Task<string> GetDatabaseConnectionStringByName(string name);
        Task<int> GetDatabaseIdByName(string name);
    }
}
