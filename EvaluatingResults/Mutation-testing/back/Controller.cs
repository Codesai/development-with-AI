using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using InterestApi.Domain;
using InterestApi.Repository;

namespace InterestApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class RegistrationController : ControllerBase
    {
        private readonly FileRegistrationRepository _repository;
        private readonly RegistrationPolicy _policy;

        public RegistrationController(
            FileRegistrationRepository repository,
            RegistrationPolicy policy)
        {
            _repository = repository;
            _policy = policy;
        }
       
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Registration registration)
        {
            if (registration == null) return BadRequest();

            registration.Email = registration.Email?.Trim() ?? string.Empty;
            registration.Course = registration.Course?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(registration.Email) || string.IsNullOrEmpty(registration.Course))
                return BadRequest();

            var registrations = await _repository.GetAllAsync();
            var confirmedRegistrations = registrations.Count(existingRegistration =>
                existingRegistration.Status == RegistrationDecision.Accepted
                && string.Equals(existingRegistration.Course.Trim(), registration.Course, StringComparison.OrdinalIgnoreCase));

            registration.Status = _policy.Decide(
                isCourseOpen: true,
                hasAcceptedTerms: registration.HasAcceptedTerms,
                confirmedRegistrations: confirmedRegistrations);

            await _repository.SaveAsync(registration);

            return Created("/api/register", new { status = registration.Status.ToString() });
        }
    }

}
