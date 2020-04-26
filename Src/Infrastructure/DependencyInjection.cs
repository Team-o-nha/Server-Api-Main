using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Interfaces;
using ColabSpace.Infrastructure.Persistence;
using ColabSpace.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ColabSpace.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddEntityFrameworkCosmos();
            services.AddDbContext<ColabSpaceDbContext>(options =>
            {
                options.UseCosmos(
                        configuration["CosmosDb:EndpointUrl"],
                        configuration["CosmosDb:PrivateKey"],
                        configuration["CosmosDb:DbName"]);
            });
            services.AddScoped<IColabSpaceDbContext>(provider => provider.GetService<ColabSpaceDbContext>());

            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}
