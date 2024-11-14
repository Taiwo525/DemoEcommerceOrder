using Ecommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interface;
using OrderApi.Infrastructure.Persistence;
using OrderApi.Infrastructure.Repositories;

namespace OrderApi.Infrastructure.Extensions
{
    public static  class ServiceExtension
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            // add database connectivity
            // add authentication scheme
            SharedServiceContainer.AddSharedService<OrderDbContext>(services, config, config["MySerilog:FileName"]);

            services.AddScoped<IOrder, OrderRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //register middleware such as:
            // global exception to handle external error,
            // listen to api gateway only to block all outsiders call
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
