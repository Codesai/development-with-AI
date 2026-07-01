namespace ArchitectureFitness.Domain.Order;

public sealed class OrderRiskPolicy
{
    public bool IsRisky(OrderRiskAssessment assessment)
    {
        return assessment.IsFlaggedByExternalRiskSystem;
    }
}
