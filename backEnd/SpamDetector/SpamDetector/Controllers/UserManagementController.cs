using Microsoft.AspNetCore.Mvc;
using MediatR;
using SpamDetector.Models.UserManagement;
using System.Net;
using SpamDetector.Features.UserManagement.Register.Dtos;
using SpamDetector.Features.UserManagement.Register.Commands.AddUser;
using Microsoft.AspNetCore.Authorization;
using SpamDetector.Features.UserManagement.Login.Dtos;
using SpamDetector.Features.UserManagement.Login.Queries.GetUser;

namespace SpamDetector.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    [Authorize]
    public class UserManagementController : ControllerBase
    {
        private readonly IMediator _mediatR;
        public UserManagementController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost("register"), AllowAnonymous]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto newUser)
        {
            var response = await _mediatR.Send(new AddUserCommand() { NewUser = newUser });
            return Ok(response);
        }

        [HttpPost("login"), AllowAnonymous]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> Login(UserLoginDto user)
        {
            var response = await _mediatR.Send(new GetUserQuery() { User = user });
            return Ok(response);
        }

    }
}
