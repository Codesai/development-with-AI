using ArchitectureFitness.Application.Orders;
using ArchitectureFitness.Domain.Order;
using ArchitectureFitness.Infrastructure.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitectureFitness.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddArchitectureFitness(this IServiceCollection services)
    {
        services.AddTransient<OrderRiskPolicy>();
        services.AddTransient<ReviewOrderRisk>();
        services.AddSingleton<HttpClient>();
        services.AddTransient<IOrderRiskGateway, HttpOrderRiskGateway>();

        return services;
    }
}
