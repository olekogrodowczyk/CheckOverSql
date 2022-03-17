namespace Application.Common.QueryEvaluation
{
    public interface IQueryBuilder
    {
        QueryBuilder AddCount();
        QueryBuilder AddLimit(int limit);
        QueryBuilder CheckOrderBy();
        QueryBuilder AddRowNumberColumn();
        QueryBuilder SetInitQuery(string query);
        string GetResult();
    }
}
