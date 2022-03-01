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
            SqlConnection connection = new SqlConnection(connectionString.Replace("\\\\", "\\"));
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            var result = await command.ExecuteNonQueryAsync();
            connection.Close();
            return result;
        }

        public async Task<int> ExecuteQueryGetOneIntValue(string query, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString.Replace("\\\\", "\\"));
            connection.Open();
            int result = 0;
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                var dr = await command.ExecuteReaderAsync();
                result = await dr.ReadAsync() ? dr.GetInt32(0) : -1;
            }
            connection.Close();
            return result;
        }

        public async Task<List<string>> ExecuteQueryGetOneRow(string query, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString.Replace("\\\\", "\\"));
            connection.Open();
            List<string> values = new List<string>();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string value = reader.IsDBNull(i) ? "NULL" : reader[i].ToString();
                    values.Add(value);
                }
            }
            connection.Close();
            return values;
        }


        public async Task<List<List<string>>> ExecuteQueryWithData(string query, string connectionString, int? numberOfRows)
        {    
            if(numberOfRows is not null) { query = limitQueryResult(query, (int)numberOfRows); }
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);            
            var queryResult =  await getDataInMatrix(command);
            var columnNames = await GetColumnNames(query, connectionString);
            queryResult.Insert(0, columnNames);

            await command.DisposeAsync();
            connection.Close();

            return queryResult;
        }

        private string limitQueryResult(string query, int numberOfRows)
        {
            query = query.TrimEnd().TrimEnd(';');
            return $"SELECT TOP({numberOfRows}) * FROM({query}) data;";
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
                        string value = reader.IsDBNull(i) ? "NULL" : reader[i].ToString();
                        RowValues.Add(value);
                    }
                    values.Add(RowValues);
                }
            }
            return values;
        }

        public async Task<List<string>> GetColumnNames(string query, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString.Replace("\\\\", "\\"));
            connection.Open();
            DataTable dataTable = null;
            using (SqlCommand command = new SqlCommand(query, connection))
            {               
                using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SchemaOnly))
                {
                    dataTable = await reader.GetSchemaTableAsync();
                }
            }              
            List<string> columnNames = new List<string>();
            dataTable?.Rows?.Cast<DataRow>().ToList().ForEach(row =>
            {
                columnNames.Add(row.ItemArray[0].ToString());
            });         
            connection.Close();
            return columnNames;
        }
    }
}
