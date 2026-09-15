namespace InterestApi.Domain;

public enum RegistrationDecision
{
    Accepted,
    Waitlisted,
    Rejected
}

public sealed class RegistrationPolicy
{
    public const int CourseCapacity = 30;

    public RegistrationDecision Decide(
        bool isCourseOpen,
        bool hasAcceptedTerms,
        int confirmedRegistrations)
    {
        if (confirmedRegistrations < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(confirmedRegistrations));
        }

        if (!isCourseOpen || !hasAcceptedTerms)
        {
            return RegistrationDecision.Rejected;
        }

        return confirmedRegistrations < CourseCapacity
            ? RegistrationDecision.Accepted
            : RegistrationDecision.Waitlisted;
    }
}
