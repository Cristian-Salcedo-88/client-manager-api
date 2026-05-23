using ClientManager.Domain.Interfaces;
using ClientManager.Infraestructure.Context;
using ClientManager.Infraestructure.Interfaces;
using ClientManager.Infraestructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientManager.Api.Extension
{
    public static class CoreExtension
    {
        public static IServiceCollection AddCoreExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISettingsContext, SettingsContext>();
            services.AddTransient<IClientRepository, ClientRepository>();

            return services;
        }
    }
}
