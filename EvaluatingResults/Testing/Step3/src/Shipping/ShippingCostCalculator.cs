namespace Shipping;

public sealed class ShippingCostCalculator
{
    public double Calculate(double weightKg, bool isPremium)
    {
        if (isPremium || weightKg < 5.0)
        {
            return 0.0;
        }

        if (weightKg <= 20.0)
        {
            return 4.99;
        }

        return 9.99;
    }
}
