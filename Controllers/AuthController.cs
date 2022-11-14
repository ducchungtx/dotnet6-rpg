using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using first_api.Data;
using first_api.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace first_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceReponse<int>>> Register(UserRegisterDto request)
        {
            var reponse = await _authRepo.Register(
                new User { Username = request.Username },
                request.Password
            );
            if (!reponse.Success)
            {
                return BadRequest(reponse);
            }
            return Ok(reponse);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceReponse<string>>> Login(UserLoginDto request)
        {
            var reponse = await _authRepo.Login(request.Username, request.Password);
            if (!reponse.Success)
            {
                return BadRequest(reponse);
            }
            return Ok(reponse);
        }
    }
}
