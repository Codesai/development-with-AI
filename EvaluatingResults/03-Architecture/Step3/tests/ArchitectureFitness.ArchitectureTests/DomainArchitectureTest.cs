using ArchitectureFitness.Domain.Order;
using NetArchTest.Rules;

namespace ArchitectureFitness.ArchitectureTests;

public sealed class DomainArchitectureTest
{
    [Fact]
    public void DomainMustNotDependOnOuterModules()
    {
        var result = Types.InAssembly(typeof(OrderRiskPolicy).Assembly)
            .Should()
            .NotHaveDependencyOnAny("ArchitectureFitness.Application", "ArchitectureFitness.Infrastructure")
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            "Domain classes must not depend on application or infrastructure modules. Keep orchestration and infrastructure integrations outside the domain.");
    }
}
