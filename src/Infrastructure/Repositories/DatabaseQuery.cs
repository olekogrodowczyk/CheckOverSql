using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DatabaseQuery : IDatabaseQuery
    {
        private readonly IConfiguration _configuration;
        private string _connectionString = string.Empty;
        

        public DatabaseQuery(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        private async Task<Dictionary<int, object>> getDataInDictionary(SqlCommand command)
        {          
            Dictionary<int,object> data = new Dictionary<int,object>();
            using(SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                int counter = 0;
                while(await reader.ReadAsync())
                {
                    var values = new Object[reader.FieldCount];
                    reader.GetValues(values);
                    data.Add(counter, values);
                    counter++;
                }
            }
            return data;
        }

        public async Task<Dictionary<int,object>> GetData(string query, string connectionString)
        {    
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);       
            var result =  await getDataInDictionary(command);

            connection.Close();

            return result;
        }
    }
}
