using Shipping;

namespace Shipping.Tests;

public sealed class ShippingCostCalculatorTest
{
    private readonly ShippingCostCalculator _calculator = new();

    [Fact]
    public void PremiumUsersGetFreeShipping()
    {
        Assert.Equal(0.0, _calculator.Calculate(20.0, isPremium: true));
    }

    [Fact]
    public void NonPremiumPackageBelowFiveKilogramsIsFree()
    {
        Assert.Equal(0.0, _calculator.Calculate(4.99, isPremium: false));
    }

    [Fact]
    public void NonPremiumPackageOfExactlyFiveKilogramsIsNotFree()
    {
        Assert.Equal(4.99, _calculator.Calculate(5.0, isPremium: false));
    }

    [Fact]
    public void StandardShippingCostsFourNinetyNine()
    {
        Assert.Equal(4.99, _calculator.Calculate(10.0, isPremium: false));
    }

    [Fact]
    public void NonPremiumPackageBelowTwentyKilogramsIsStandardShipping()
    {
        Assert.Equal(4.99, _calculator.Calculate(19.99, isPremium: false));
    }

    [Fact]
    public void NonPremiumPackageOfExactlyTwentyKilogramsIsHeavyShipping()
    {
        Assert.Equal(9.99, _calculator.Calculate(20.0, isPremium: false));
    }

    [Fact]
    public void HeavyPackagesCostNineNinetyNine()
    {
        Assert.Equal(9.99, _calculator.Calculate(30.0, isPremium: false));
    }
}
