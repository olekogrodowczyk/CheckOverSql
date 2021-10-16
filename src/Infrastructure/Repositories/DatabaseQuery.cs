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

        private void setConnectionString(ExerciseDatabaseEnum exerciseDatabaseEnum)
        {
            switch (exerciseDatabaseEnum)
            {
                case ExerciseDatabaseEnum.FootballLeague:
                    _connectionString = _configuration.GetConnectionString("FootballLeagueDb");
                    break;
                case ExerciseDatabaseEnum.AdventureWorks:
                    break;
                default:
                    throw new BadRequestException("Coś poszło nie tak");
            }
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

        public async Task<object> GetData(string query, ExerciseDatabaseEnum exerciseDatabaseEnum)
        {    
            setConnectionString(exerciseDatabaseEnum);
            SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);       
            var result =  await getDataInDictionary(command);

            connection.Close();

            return result;
        }
    }
}
