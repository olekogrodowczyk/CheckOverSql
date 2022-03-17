using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation
{
    public interface IQueryEvaluatorDriver
    {
        Task<bool> Evaluate(string query1, string query2, string connectionString);
    }
}