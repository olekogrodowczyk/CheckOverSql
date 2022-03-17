using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    //Must be injected as scoped
    public class ConnectionStringService : IConnectionStringService
    {
        private string _connectionString;

        public string ConnectionString
        {
            get { return _connectionString; }
            set { if (!String.IsNullOrEmpty(value)) { _connectionString = value; } }
        }

    }
}
