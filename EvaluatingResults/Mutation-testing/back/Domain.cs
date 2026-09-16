namespace InterestApi.Domain
{
    public class Registration
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Course { get; set; } = null!;
        public bool HasAcceptedTerms { get; set; }
        public RegistrationDecision Status { get; set; } = RegistrationDecision.Accepted;
    }
}
