using Application.Common.QueryEvaluation;
using Application.Common.QueryEvaluation.Handlers;
using Application.Common.QueryEvaluation.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjectionQueryEvaluator
    {
        public static IServiceCollection AddQueryEvaluator(this IServiceCollection services)
        {
            services.AddScoped<IQueryEvaluatorDriver, QueryEvaluatorDriverOptimized>();
            services.AddTransient<IQueryBuilder, QueryBuilder>();
            services.AddScoped<IConnectionStringService, ConnectionStringService>();
            services.AddScoped<IQueryEvaluatorService, QueryEvaluatorService>();
            services.AddScoped<IQueryEvaluationLogging, QueryEvaluationLogging>();

            //Order of below injections matters
            services.AddScoped<IEvaluationHandler, BodiesHandler>();
            services.AddScoped<IEvaluationHandler, ColumnsHandler>();
            services.AddScoped<IEvaluationHandler, CountsHandler>();
            services.AddScoped<IEvaluationHandler, IntersectHandler>();
            services.AddScoped<IEvaluationHandler, FirstMiddleLastRowsHandler>();
            return services;
        }
    }
}
