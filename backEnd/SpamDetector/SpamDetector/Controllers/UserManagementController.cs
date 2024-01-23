using Microsoft.AspNetCore.Mvc;
using MediatR;
using SpamDetector.Models.UserManagement;
using System.Net;
using SpamDetector.Features.UserManagement.Register.Dtos;
using SpamDetector.Features.UserManagement.Register.Commands.AddUser;

namespace SpamDetector.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IMediator _mediatR;
        public UserManagementController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto newUser)
        {
            var response = await _mediatR.Send(new AddUserCommand() { NewUser = newUser });
            return Ok(response);
        }

        //[HttpPost("login")]
        //public async Task<ActionResult<string>> Login(UserLoginDto request)
        //{
        //    if(user.UserName != request.UserName)
        //    {
        //        return BadRequest("User not found");
        //    }

        //    if(!VerifyPasswordHash(request.Password,user.PassWordHash,user.PassWordSalt))
        //    {
        //        return BadRequest("Bad Password");
        //    }

        //    string token = CreateToken(user);
        //    return Ok(token);
        //}

    }
}
