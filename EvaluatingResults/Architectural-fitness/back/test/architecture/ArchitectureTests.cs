#if ARCHITECTURE_TESTS
using InterestApi.Domain;
using NetArchTest.Rules;
using Xunit;

namespace InterestApi.ArchitectureTests;

public class ArchitectureTests
{
    private const string ControllersNamespace = "InterestApi.Controllers";
    private const string DomainNamespace = "InterestApi.Domain";
    private const string RepositoryNamespace = "InterestApi.Repository";

    [Fact]
    public void Controllers_can_only_access_the_domain_layer()
    {
        var result = Types.InAssembly(typeof(Registration).Assembly)
            .That()
            .ResideInNamespace(ControllersNamespace)
            .ShouldNot()
            .HaveDependencyOn(RepositoryNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, FormatFailingTypes(result));
    }

    [Fact]
    public void Domain_does_not_access_controllers()
    {
        var result = Types.InAssembly(typeof(Registration).Assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOn(ControllersNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, FormatFailingTypes(result));
    }

    private static string FormatFailingTypes(TestResult result) =>
        $"Architectural rule failed for: {string.Join(", ", result.FailingTypeNames ?? [])}";
}
#endif
