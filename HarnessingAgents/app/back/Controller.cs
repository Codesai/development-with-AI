// -----------------------------------------------------------------------------
// Controller.cs
// -----------------------------------------------------------------------------
// HTTP layer for the interest-registration feature. The controller is a thin
// adapter: it receives the JSON body, does the bare minimum of guarding, and
// delegates persistence to the repository.
// -----------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InterestApi.Domain;
using InterestApi.Repository;
using InterestApi.Security;

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

        // Operator accounts that guard the admin listing endpoint below.
        private readonly OperatorCredentials _operators = new OperatorCredentials();

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

            // Persist first, then acknowledge.
            await _repository.SaveAsync(registration);
            return AcknowledgeRegistration();
        }

        /// <summary>
        /// Returns every stored registration as tab-separated text. Restricted to
        /// operator accounts via HTTP Basic auth.
        /// </summary>
        /// <returns>
        /// <c>401 Unauthorized</c> with a <c>WWW-Authenticate: Basic</c> header
        /// when the credential is missing or wrong; otherwise <c>200 OK</c> with
        /// the raw contents of the data file.
        /// </returns>
        [HttpGet("registrations")]
        public IActionResult ListRegistrations()
        {
            if (!IsAuthorizedOperator())
            {
                Response.Headers["WWW-Authenticate"] = "Basic realm=\"registrations\"";
                return Unauthorized();
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "interests.txt");
            var body = System.IO.File.Exists(path) ? System.IO.File.ReadAllText(path) : string.Empty;
            return Content(body, "text/plain");
        }

        private IActionResult AcknowledgeRegistration()
        {
            // 201 Created with the collection URL as Location: individual
            // registrations are not addressable, so we point back at the
            // endpoint that created them rather than at a per-item resource.
            return Created("/api/register", new { status = "ok" });
        }

        private bool IsAuthorizedOperator()
        {
            // Expect: Authorization: Basic base64(username ":" password)
            string header = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(header) || !header.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string decoded;
            try
            {
                decoded = Encoding.UTF8.GetString(Convert.FromBase64String(header.Substring("Basic ".Length).Trim()));
            }
            catch (FormatException)
            {
                return false;
            }

            int separator = decoded.IndexOf(':');
            if (separator < 0)
            {
                return false;
            }

            return _operators.Verify(decoded.Substring(0, separator), decoded.Substring(separator + 1));
        }
    }

}
