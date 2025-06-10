using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TaskManagement.DAL;
using TaskManagement.Factory;
using TaskManagement.IRepository;
using TaskManagement.Models;
using TaskManagement.Repository;
using TaskManagement.Service;

namespace TaskManagement.DI
{
    /// <summary>
    /// Register Services
    /// </summary>
    public static class DIConfig
    {
        public static IServiceCollection RegisterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<TaskManagementDBContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(typeof(MappingProfile));

            /// User Management
            services.AddTransient<IUserManagementService, UserManagementService>();
            services.AddTransient<IUserManagementRepository, UserManagementRepository>();

            /// Task Management
            services.AddTransient<ITaskManagementService, TaskManagementService>();
            services.AddTransient<ITaskManagementRepository, TaskManagementRepository>();

            return services;
        }
    }
}
