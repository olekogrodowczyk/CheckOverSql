using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class ConnectionString : ValueObject
    {
        public string Server { get; private set; }
        public string Database { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }

        public ConnectionString()
        {

        }

        public ConnectionString(string server, string database, string user, string password)
        {
            Server = server;
            Database = database;
            User = user;
            Password = password;
        }

        public override string ToString()
        {
            return $"Server={Server};Database={Database};User Id={User};Password={Password}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Server;
            yield return Database;
            yield return User;  
            yield return Password;
        }
    }
}
