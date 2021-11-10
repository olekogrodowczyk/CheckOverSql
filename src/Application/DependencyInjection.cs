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

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<ISolutionService , SolutionService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IDataComparerService, DataComparerSercice>();
            services.AddScoped<IInvitationService, InvitationService>();
            return services;
        }
    }
}
