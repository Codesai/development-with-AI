namespace InterestApi.Domain
{
    public sealed class RegistrationRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Course { get; set; }
        public bool HasAcceptedTerms { get; set; }
    }

    public class Registration
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Course { get; set; } = null!;
        public bool HasAcceptedTerms { get; set; }
        public RegistrationDecision Status { get; set; } = RegistrationDecision.Accepted;
    }
}
