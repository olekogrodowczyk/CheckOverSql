using Application.Interfaces;
using Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Reflection;
using Application.Services;
using Domain.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Application.Authorization;
using MediatR;
using Application.Common.Behaviours;
using FluentValidation;
using Infrastructure.Authorization;
using Application.Common.QueryEvaluation;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);         
            services.AddScoped<IAuthorizationHandler, GetSolvingByIdRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ISolutionService , SolutionService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IDataComparerService, DataComparerService>();
            services.AddScoped<IQueryEvaluator, QueryEvaluator>();
            services.AddScoped<IQueryEvaluatorDriver, QueryEvaluatorDriverOptimized>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavour<>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            return services;
        }
    }
}
