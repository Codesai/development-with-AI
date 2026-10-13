using InterestApi.Controllers;
using NetArchTest.Rules;

namespace InterestApi.Tests.Architecture;

public sealed class LayerDependencyTests
{
    private const string ControllersNamespace = "InterestApi.Controllers";
    private const string RepositoryNamespace = "InterestApi.Repository";
    private const string DomainNamespace = "InterestApi.Domain";

    [Fact]
    public void ControllersDoNotDependOnRepository() =>
        AssertNoDependency(from: ControllersNamespace, to: RepositoryNamespace);

    [Fact]
    public void RepositoryDoesNotDependOnControllers() =>
        AssertNoDependency(from: RepositoryNamespace, to: ControllersNamespace);

    [Fact]
    public void DomainDoesNotDependOnControllers() =>
        AssertNoDependency(from: DomainNamespace, to: ControllersNamespace);

    private static void AssertNoDependency(string from, string to)
    {
        var result = Types.InAssembly(typeof(RegistrationController).Assembly)
            .That().ResideInNamespace(from)
            .ShouldNot().HaveDependencyOn(to)
            .GetResult();

        var offenders = result.FailingTypeNames ?? [];
        Assert.True(result.IsSuccessful,
            $"{from} must not access {to}. Offending types: {string.Join(", ", offenders)}");
    }
}
