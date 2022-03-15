using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    public interface IQueryEvaluator
    {
        Task<bool> CompareColumnNames(string query1, string query2);
        Task<int> GetCountOfQuery(string query);
        Task<List<List<string>>> GetFirstMiddleLastRows(string query, int queryResultCount);
        Task<int> GetIntersectQueryCount(string query1, string query2);
        void InitConnectionString(string connectionString);
    }
}