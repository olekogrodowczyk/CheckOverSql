using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<int> ExecuteQueryNoData(string query, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            var result = await command.ExecuteNonQueryAsync();
            connection.Close();
            return result;
        }

        public async Task<List<List<string>>> ExecuteQueryWithData(string query, string connectionString)
        {    
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);            
            var queryResult =  await getDataInMatrix(command);
            var columnNames = await getColumnNames(command);
            queryResult.Insert(0, columnNames);

            await command.DisposeAsync();
            connection.Close();

            return queryResult;
        }

        private async Task<List<List<string>>> getDataInMatrix(SqlCommand command)
        {
            List<List<string>> values = new List<List<string>>();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    List<string> RowValues = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string value = reader[i].ToString();
                        RowValues.Add(value);
                    }
                    values.Add(RowValues);
                }
            }
            return values;
        }

        private async Task<List<string>> getColumnNames(SqlCommand command)
        {
            DataTable dataTable = null;
            using(SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SchemaOnly))
            {
                dataTable = await reader.GetSchemaTableAsync();
            }
            List<string> columnNames = new List<string>();
            dataTable?.Rows?.Cast<DataRow>().ToList().ForEach(row =>
            {
                columnNames.Add(row.ItemArray[0].ToString());
            });         
               
            return columnNames;
        }
    }
}
