using ArchitectureFitness.Domain.Order;
using NetArchTest.Rules;
using System.Reflection;

namespace ArchitectureFitness.ArchitectureTests;

public sealed class DomainArchitectureTest
{
    private static readonly Assembly[] DomainAssemblies =
    [
        typeof(OrderRiskPolicy).Assembly
    ];

    [Fact]
    public void DomainMustNotDependOnInfrastructure()
    {
        var result = Types.InAssemblies(DomainAssemblies)
            .Should()
            .NotHaveDependencyOnAny("ArchitectureFitness.Infrastructure", "System.Net.Http", "System.Net.Http.Json")
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            "Domain classes must not depend on infrastructure. Keep infrastructure concerns outside the domain.");
    }
}
