﻿using Microsoft.AspNetCore.Mvc;
using MediatR;
using SpamDetector.Models.UserManagement;
using System.Net;
using SpamDetector.Features.UserManagement.Register.Dtos;
using SpamDetector.Features.UserManagement.Register.Commands.AddUser;
using Microsoft.AspNetCore.Authorization;
using SpamDetector.Features.UserManagement.Login.Queries.GetUser;
using SpamDetector.Features.UserManagement.Login.Commands.UpdateRefreshTokenByUser;
using SpamDetector.Features.UserManagement.ResetPassword.Commands.AddPasswordResetToken;
using SpamDetector.Features.UserManagement.ResetPassword.Commands.UpdatePassword;

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
        public async Task<ActionResult> Register(UserRegisterDto newUser)
        {
            var response = await _mediatR.Send(new AddUserCommand() { NewUser = newUser });
            return Ok(response);
        }

        [HttpPost("login"), AllowAnonymous]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> Login(UserLogin user)
        {
            var response = await _mediatR.Send(new GetUserQuery() { User = user });
            return Ok(response);
        }

        [HttpPost("refresh-token"), AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> RefreshToken(UserLogin user)
        {
            await _mediatR.Send(new UpdateRefreshTokenByUserCommand() { User = user });
            return Ok();
        }

        [HttpPost("forgot-password"), AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            await _mediatR.Send(new AddPasswordResetTokenCommand() { Email = email });
            return Ok();
        }

        [HttpPost("reset-password"), AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ResetPassword(UserPasswordReset userPasswordReset)
        {
            await _mediatR.Send(new UpdatePasswordCommand() { User = userPasswordReset });
            return Ok();
        }
    }
}
