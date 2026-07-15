using ArchitectureFitness.Domain.Order;
using NetArchTest.Rules;

namespace ArchitectureFitness.ArchitectureTests;

public sealed class DomainArchitectureTest
{
    [Fact]
    public void DomainMustNotDependOnInfrastructureConcerns()
    {
        var result = Types.InAssembly(typeof(OrderRiskPolicy).Assembly)
            .Should()
            .NotHaveDependencyOnAny("ArchitectureFitness.Infrastructure", "System.Net.Http", "System.Net.Http.Json")
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            "Domain classes must not depend on infrastructure or HTTP concerns. Move external communication and infrastructure integrations to the infrastructure layer.");
    }
}
