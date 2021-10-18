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

        
        private async Task<List<List<string>>> getDataInDictionary(SqlCommand command)
        {
            List<List<string>> values = new List<List<string>>();         
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while(await reader.ReadAsync())
                {
                    List<string> RowValues = new List<string>();
                    for (int i= 0; i < reader.FieldCount; i++)
                    {
                        string value = reader[i].ToString();
                        RowValues.Add(value);
                    }
                    values.Add(RowValues);
                }
            }
            return values;
        }

        public async Task<List<List<string>>> GetData(string query, string connectionString)
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
