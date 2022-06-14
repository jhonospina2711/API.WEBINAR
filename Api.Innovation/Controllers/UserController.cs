using Entities;
using Business.Interfaces.General;
using Entities.Enums;
using Entities.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using Business.Interfaces.Base;


namespace Api.Covid19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsers _userService;

        private readonly IValidator<User> _entityValidator;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;


        public UserController(IUsers userService, IValidator<User> entityValidator, IResponseService responseService, IExceptionHandler exceptionHandler)
        {
            _userService = userService;
            _entityValidator = entityValidator;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
        }


        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationModel param)
        {
            if (param != null)
            {
                if (!ModelState.IsValid) return BadRequest(MessagesEnum.InvalidModel);
                Singleton.Instance.Audit = false;
                var response = await _userService.Authenticate(param.Email, param.Password);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }

        [HttpGet("ValidateUserActivate")]
        public async Task<IActionResult> ValidateUserActivate(string email)
        {
            if (email != null)
            {
                if (!ModelState.IsValid) return BadRequest(MessagesEnum.InvalidModel);
                Singleton.Instance.Audit = false;
                var response = await _userService.ValidateUserActivate(email);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPut("CreatePassword")]
        public async Task<IActionResult> CreatePassword([FromBody] SetPasswordModel param)
        {
            if (param != null)
            {
                if (!ModelState.IsValid) return BadRequest(MessagesEnum.InvalidModel);
                Singleton.Instance.Audit = false;
                var response = await _userService.CreatePassword(param);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }

        [HttpGet("ForgotUserpassword")]
        public async Task<IActionResult> ForgotUserpassword(string email)
        {
            if (email != null)
            {
                if (!ModelState.IsValid) return BadRequest(MessagesEnum.InvalidModel);
                Singleton.Instance.Audit = false;
                var response = await _userService.ForgotUserpassword(email);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }

        [HttpGet("SendCodeSupport")]
        public async Task<IActionResult> SendCodeSupport(string email)
        {
            if (email != null)
            {
                if (!ModelState.IsValid) return BadRequest(MessagesEnum.InvalidModel);
                Singleton.Instance.Audit = false;
                var response = await _userService.SendCodeSupport(email);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }

    }
}