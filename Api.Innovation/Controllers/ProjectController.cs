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
    public class ProjectController : ControllerBase
    {
        private readonly IProjects _projects;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
       

        public ProjectController(IProjects projects, IResponseService responseService, IExceptionHandler exceptionHandler)
        {
            _projects = projects;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
        }

        [HttpGet("ListProjects")]
        public async Task<IActionResult> ListProjects()
        {
            Singleton.Instance.Audit = false;
            var response = await _projects.List();
            if (response.Meta.Status) return Ok(response);
            ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
            return BadRequest(ModelState);

        }

    }
}