using InterestApi.Domain;

namespace InterestApi.Tests;

public sealed class RegistrationPolicyTests
{
    private readonly RegistrationPolicy _policy = new();

    [Fact]
    public void ClosedCourseIsRejectedEvenWhenSeatsAreAvailable() =>
        Assert.Equal(RegistrationDecision.Rejected, _policy.Decide(false, true, 0));

    [Fact]
    public void MissingTermsAcceptanceIsRejected() =>
        Assert.Equal(RegistrationDecision.Rejected, _policy.Decide(true, false, 0));

    [Fact]
    public void OpenCourseBelowCapacityIsAccepted() =>
        Assert.Equal(RegistrationDecision.Accepted, _policy.Decide(true, true, 29));

    [Fact]
    public void CourseAtOrAboveCapacityIsWaitlisted() =>
        Assert.Equal(RegistrationDecision.Waitlisted, _policy.Decide(true, true, 31));

    [Fact]
    public void NegativeRegistrationCountIsInvalid() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => _policy.Decide(true, true, -1));
}
