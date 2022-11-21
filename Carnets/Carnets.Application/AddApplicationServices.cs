using Carnets.Application.Interfaces;
using Carnets.Application.Services;
using Carnets.Application.SpecificPermissions.Commands;
using Carnets.Application.SpecificPermissions.Queries;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace Carnets.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Generic [Specific permissions]
            services.AddScoped<IRequestHandler<CreatePermissionCommand<ClassPermission>, Result<ClassPermission>>, CreatePermissionCommandHandler<ClassPermission>>();
            services.AddScoped<IRequestHandler<CreatePermissionCommand<PerkPermission>, Result<PerkPermission>>, CreatePermissionCommandHandler<PerkPermission>>();

            services.AddScoped<IRequestHandler<DeletePermissionCommand<ClassPermission>, Result<bool>>, DeletePermissionCommandHandler<ClassPermission>>();
            services.AddScoped<IRequestHandler<DeletePermissionCommand<PerkPermission>, Result<bool>>, DeletePermissionCommandHandler<PerkPermission>>();

            services.AddScoped<IRequestHandler<GetAllGympassTypePermissionsQuery<ClassPermission>, IEnumerable<ClassPermission>>, GetAllGympassTypePermissionsQueryHandler<ClassPermission>>();
            services.AddScoped<IRequestHandler<GetAllGympassTypePermissionsQuery<PerkPermission>, IEnumerable<PerkPermission>>, GetAllGympassTypePermissionsQueryHandler<PerkPermission>>();

            services.AddScoped<IRequestHandler<GetAllPermisionsQuery<ClassPermission>, IEnumerable<ClassPermission>>, GetAllPermisionsQueryHandler<ClassPermission>>();
            services.AddScoped<IRequestHandler<GetAllPermisionsQuery<PerkPermission>, IEnumerable<PerkPermission>>, GetAllPermisionsQueryHandler<PerkPermission>>();

            services.AddScoped<IRequestHandler<GetPermissionById<ClassPermission>, ClassPermission>, GetPermissionByIdHandler<ClassPermission>>();
            services.AddScoped<IRequestHandler<GetPermissionById<PerkPermission>, PerkPermission>, GetPermissionByIdHandler<PerkPermission>>();

            // Add services
            services.AddScoped<JwtSecurityTokenHandler>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IEntryTokenService, EntryTokenService>();

            return services;
        }
    }
}
