namespace ArchitectureFitness.Domain.Order;

public sealed record OrderRiskAssessment(string OrderId, bool IsFlaggedByExternalRiskSystem);
