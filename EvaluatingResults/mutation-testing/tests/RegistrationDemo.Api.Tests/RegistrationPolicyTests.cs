using RegistrationDemo.Api.Domain;

namespace RegistrationDemo.Api.Tests;

public sealed class RegistrationPolicyTests
{
    private readonly RegistrationPolicy _policy = new();

    [Fact]
    public void ClosedCourseIsRejectedEvenWhenSeatsAreAvailable() =>
        Assert.Equal(RegistrationDecision.Rejected, _policy.Decide(false, true, 0));

    [Fact]
    public void MissingTermsAcceptanceIsRejected() =>
        Assert.Equal(RegistrationDecision.Rejected, _policy.Decide(true, false, 0));

    [Theory]
    [InlineData(0)]
    [InlineData(RegistrationPolicy.CourseCapacity - 1)]
    public void OpenCourseBelowCapacityIsAccepted(int confirmedRegistrations) =>
        Assert.Equal(RegistrationDecision.Accepted, _policy.Decide(true, true, confirmedRegistrations));

    [Theory]
    [InlineData(RegistrationPolicy.CourseCapacity)]
    [InlineData(RegistrationPolicy.CourseCapacity + 1)]
    public void CourseAtOrAboveCapacityIsWaitlisted(int confirmedRegistrations) =>
        Assert.Equal(RegistrationDecision.Waitlisted, _policy.Decide(true, true, confirmedRegistrations));

    [Fact]
    public void NegativeRegistrationCountIsInvalid() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => _policy.Decide(true, true, -1));
}
