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
    public class InvestmentProjectController : ControllerBase
    {
        private readonly IUserInvestmentProjects _userInvestmentProjects;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;


        public InvestmentProjectController(IUserInvestmentProjects userInvestmentProjects, IResponseService responseService, IExceptionHandler exceptionHandler)
        {
            _userInvestmentProjects = userInvestmentProjects;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserInvestmentProjectModel param)
        {
            if (param != null)
            {
                if (!ModelState.IsValid) return BadRequest(MessagesEnum.InvalidModel);
                Singleton.Instance.Audit = false;
                var response = await _userInvestmentProjects.CreateInvestmentProject(param);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("GetProjectsCoinsById")]
        public async Task<IActionResult> GetProjectsCoinsById(int projectId)
        {
            if (projectId != 0)
            {
                Singleton.Instance.Audit = false;
                var response = await _userInvestmentProjects.GetByProjectId(projectId);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("GetUserCoinsWinsByUserId")]
        public async Task<IActionResult> GetUserCoinsWinsByUserId(string userWalletAccountId)
        {
            if (userWalletAccountId != null)
            {
                Singleton.Instance.Audit = false;
                var response = await _userInvestmentProjects.GetUserCoinsWinsByUserId(userWalletAccountId);
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
            var response = await _userInvestmentProjects.List();
            if (response.Meta.Status) return Ok(response);
            ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
            return BadRequest(ModelState);

        }

    }
}