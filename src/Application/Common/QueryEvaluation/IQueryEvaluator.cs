using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    public interface IQueryEvaluator
    {
        Task<bool> CompareColumnNames(string query1, string query2, string connectionString);
        Task<int> GetCountOfQuery(string query, string connectionString);
        Task<List<List<string>>> GetFirstMiddleLastRows(string query, int queryResultCount, string connectionString);
        Task<int> GetIntersectQueryCount(string query1, string query2, string connectionString);
    }
}