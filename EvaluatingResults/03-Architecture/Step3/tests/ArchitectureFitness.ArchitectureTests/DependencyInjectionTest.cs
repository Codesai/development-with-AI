using ArchitectureFitness.Application.Orders;
using ArchitectureFitness.Infrastructure;
using ArchitectureFitness.Infrastructure.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitectureFitness.ArchitectureTests;

public sealed class DependencyInjectionTest
{
    [Fact]
    public void CompositionRootCanResolveReviewOrderRisk()
    {
        var services = new ServiceCollection();

        services.AddArchitectureFitness();

        using var provider = services.BuildServiceProvider(
            new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

        var useCase = provider.GetRequiredService<ReviewOrderRisk>();
        var gateway = provider.GetRequiredService<IOrderRiskGateway>();

        Assert.NotNull(useCase);
        Assert.IsType<HttpOrderRiskGateway>(gateway);
    }
}
