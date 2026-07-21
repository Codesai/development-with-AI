using ArchitectureFitness.Domain.Order;
using NetArchTest.Rules;
using System.Reflection;

namespace ArchitectureFitness.ArchitectureTests;

public sealed class DomainArchitectureTest
{
    private static readonly Assembly[] DomainAssemblies =
    [
        typeof(Order).Assembly
    ];

    [Fact]
    public void DomainMustNotDependOnOuterModules()
    {
        var result = Types.InAssemblies(DomainAssemblies)
            .Should()
            .NotHaveDependencyOnAny("ArchitectureFitness.Application", "ArchitectureFitness.Infrastructure")
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            "Domain classes must not depend on application or infrastructure modules. Keep orchestration and infrastructure integrations outside the domain.");
    }
}
