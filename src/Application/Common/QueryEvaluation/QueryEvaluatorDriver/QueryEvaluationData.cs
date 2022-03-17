namespace Application.Common.QueryEvaluation
{
    public class QueryEvaluationData
    {
        public int InvokingUserId { get; set; }
        public QueryEvaluationPhase Phase { get; set; }
        public string Query1 { get; set; }
        public string Query2 { get; set; }
        public int? Query1Count { get; set; }
        public int? Query2Count { get; set; }
        public int? IntersectCount { get; set; }
        public bool Stop { get; set; } = false;
        public bool FinalResult { get; set; }
    }
}
