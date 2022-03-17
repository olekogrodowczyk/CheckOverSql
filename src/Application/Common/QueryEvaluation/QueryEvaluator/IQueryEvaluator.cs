using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    public interface IQueryEvaluator
    {
        bool CompareBodies(string query1, string query2);
        Task<bool> CompareColumnNames(string query1, string query2);
        Task<bool> CompareFirstMiddleLastRows(string query1, string query2);
        Task<bool> CompareQueriesCount(string query1, string query2);
    }
}