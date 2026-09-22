using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly FileRegistrationRepository _repository = new FileRegistrationRepository();
       
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Registration registration)
        {
            if (registration == null) return BadRequest();

            registration.Email = registration.Email?.Trim() ?? string.Empty;
            registration.Course = registration.Course?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(registration.Email) || string.IsNullOrEmpty(registration.Course))
                return BadRequest();

            var registrations = await _repository.GetAllAsync();
            var isDuplicate = registrations.Any(existingRegistration =>
                string.Equals(existingRegistration.Email.Trim(), registration.Email, StringComparison.OrdinalIgnoreCase)
                && string.Equals(existingRegistration.Course.Trim(), registration.Course, StringComparison.OrdinalIgnoreCase));

            if (!isDuplicate)
                await _repository.SaveAsync(registration);

            return Created("/api/register", new { status = "ok" });
        }
    }

}
