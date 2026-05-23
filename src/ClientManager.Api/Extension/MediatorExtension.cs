using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ClientManager.Api.Extension
{
    public static class MediatorExtension
    {
        public static IServiceCollection AddMediatorExtension(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
            return services;
        }
    }
}
