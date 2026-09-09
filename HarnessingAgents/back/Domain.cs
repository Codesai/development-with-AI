// -----------------------------------------------------------------------------
// Domain.cs
// -----------------------------------------------------------------------------
// Domain model for the interest-registration feature. Right now the domain is a
// single data-carrying class with no behaviour.
// -----------------------------------------------------------------------------

namespace InterestApi.Domain
{
    /// <summary>
    /// One person's expression of interest in a course.
    /// </summary>
    /// <remarks>
    /// The properties are populated by the JSON model binder, so the names here
    /// must match the field names posted by the frontend form
    /// (<c>name</c>, <c>email</c>, <c>course</c>).
    /// </remarks>
    public class Registration
    {
        // The "= null!" suppresses the nullable-reference warning: the binder is
        // trusted to set these before the object is used.

        /// <summary>Full name as typed by the visitor.</summary>
        public string Name { get; set; } = null!;

        /// <summary>Contact email address. Not validated at this stage.</summary>
        public string Email { get; set; } = null!;

        /// <summary>The course the visitor is interested in.</summary>
        public string Course { get; set; } = null!;
    }
}
