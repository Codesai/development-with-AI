using Microsoft.AspNetCore.Mvc;
using System;
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

            await _repository.SaveAsync(registration);
            return Created("/api/register", new { status = "ok" });
        }
    }

}

