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
    public class UserOneTimePassController : ControllerBase
    {
        private readonly IUsersOneTimePass _usersOneTimePass;

        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;


        public UserOneTimePassController(IUsersOneTimePass usersOneTimePass, IResponseService responseService, IExceptionHandler exceptionHandler)
        {
            _usersOneTimePass = usersOneTimePass;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
        }


        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ValidateOTP([FromBody] UserOtpModel param)
        {
            if (param != null)
            {
                if (!ModelState.IsValid) return BadRequest(MessagesEnum.InvalidModel);
                Singleton.Instance.Audit = false;
                var response = await _usersOneTimePass.ValidateOTP(param.UserId, param.Otp);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }

    }
}