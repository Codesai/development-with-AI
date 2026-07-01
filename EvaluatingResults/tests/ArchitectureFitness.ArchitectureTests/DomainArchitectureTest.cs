using ArchitectureFitness.Domain.Order;
using NetArchTest.Rules;

namespace ArchitectureFitness.ArchitectureTests;

public sealed class DomainArchitectureTest
{
    [Fact]
    public void DomainMustNotDependOnHttpClients()
    {
        var result = Types.InAssembly(typeof(OrderRiskPolicy).Assembly)
            .Should()
            .NotHaveDependencyOnAny("System.Net.Http", "System.Net.Http.Json")
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            "Domain classes must not depend on HTTP clients. Move external communication to infrastructure.");
    }
}
