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
using Application.Common.Services;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IAuthorizationHandler, GetSolvingByIdRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<ISolutionService , SolutionService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IDataComparerService, DataComparerSercice>();
            services.AddScoped<IInvitationService, InvitationService>();

            return services;
        }
    }
}
