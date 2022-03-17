using System.Threading.Tasks;

namespace Application.Common.QueryEvaluation.Handlers
{
    public interface IEvaluationHandler
    {
        Task Handle(QueryEvaluationData data);
    }
}