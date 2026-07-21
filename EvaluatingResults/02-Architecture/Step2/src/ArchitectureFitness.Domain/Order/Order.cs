namespace ArchitectureFitness.Domain.Order;

public sealed record Order(string Id)
{
    public bool IsRisky(OrderRiskAssessment assessment)
    {
        if (assessment.OrderId != Id)
        {
            return false;
        }

        return assessment.IsFlaggedByExternalRiskSystem;
    }
}
