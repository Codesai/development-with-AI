// -----------------------------------------------------------------------------
// Controller.cs
// -----------------------------------------------------------------------------
// HTTP layer for the interest-registration feature. The controller is a thin
// adapter: it receives the JSON body, does the bare minimum of guarding, and
// delegates persistence to the repository.
// -----------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using InterestApi.Domain;
using InterestApi.Repository;

namespace InterestApi.Controllers
{
    /// <summary>
    /// Handles registration submissions coming from the public interest form.
    /// </summary>
    /// <remarks>
    /// Routes are attribute-based. The controller-level <c>[Route("api")]</c>
    /// prefix combines with the action-level <c>[HttpPost("register")]</c> to
    /// produce the final path <c>POST /api/register</c>.
    /// </remarks>
    [ApiController]
    [Route("api")]
    public class RegistrationController : ControllerBase
    {
        // The repository is created directly here rather than injected. This
        // keeps the sample small; a real service would register the repository
        // in the DI container and take it as a constructor parameter.
        private readonly FileRegistrationRepository _repository = new FileRegistrationRepository();

        /// <summary>
        /// Accepts a single registration and appends it to the data file.
        /// </summary>
        /// <param name="registration">
        /// The registration payload, bound from the request's JSON body.
        /// </param>
        /// <returns>
        /// <c>400 Bad Request</c> when the body is missing or unparseable;
        /// otherwise <c>201 Created</c> with a small status object.
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Registration registration)
        {
            // Guard clause: model binding yields null when the body is absent or
            // not valid JSON. Nothing to persist in that case.
            if (registration == null) return BadRequest();

            // Delegate the actual write to the repository.
            await _repository.SaveAsync(registration);

            // Report success. The location points back at the collection
            // endpoint because individual registrations are not addressable.
            return Created("/api/register", new { status = "ok" });
        }
    }

}
