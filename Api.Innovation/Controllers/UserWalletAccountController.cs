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
    public class UserWalletAccountController : ControllerBase
    {
        private readonly IUserWalletAccounts _userWalletAccounts;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;


        public UserWalletAccountController(IUserWalletAccounts userWalletAccounts, IResponseService responseService, IExceptionHandler exceptionHandler)
        {
            _userWalletAccounts = userWalletAccounts;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
        }

        [Authorize]
        [HttpPost("CreateWallet")]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletModel param)
        {
            if (param != null)
            {
                if (!ModelState.IsValid) return BadRequest(MessagesEnum.InvalidModel);
                Singleton.Instance.Audit = false;
                var response = await _userWalletAccounts.CreateWallet(param);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("GetFromId")]
        public async Task<IActionResult> GetFromId(int userId)
        {
            if (userId > 0)
            {
                Singleton.Instance.Audit = false;
                var response = await _userWalletAccounts.GetByUserId(userId);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("ListBestInvestment")]
        public async Task<IActionResult> ListBestInvestment()
        {
            Singleton.Instance.Audit = false;
            var response = await _userWalletAccounts.List();
            if (response.Meta.Status) return Ok(response);
            ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
            return BadRequest(ModelState);

        }

    }
}