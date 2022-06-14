using Entities;
using Business.Interfaces.General;
using Entities.Enums;
using Entities.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using Business.Interfaces.Base;
using System;

namespace Api.Covid19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfitabilityController : ControllerBase
    {
        private readonly IProfitabilities _profitabilities;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly GenericUtil _genericUtil = new GenericUtil();

        public ProfitabilityController(IProfitabilities profitabilities, IResponseService responseService, IExceptionHandler exceptionHandler)
        {
            _profitabilities = profitabilities;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
        }

        [Authorize]
        [HttpGet("GetProfitability")]
        public async Task<IActionResult> GetProfitability(int profitabilityId)
        {
            if (profitabilityId > 0)
            {
                Singleton.Instance.Audit = false;
                var response = await _profitabilities.Get(profitabilityId);
                if (response.Meta.Status) return Ok(response);
                ModelState.AddModelError("Error", string.Join(",", response.Meta.Errors));
                return BadRequest(ModelState);
            }
            return BadRequest();
        }


        [HttpPost("CreateDailyProfitabilities")]
        public async Task<IActionResult> CreateDailyProfitabilities()
        {
            try
            {
                int percentaje = 0;
                IResponseService response = null;
                Profitability profitability;
                for (int i = 0; i <= 23; i++)
                {
                    percentaje = new Random().Next(1, 11);
                    profitability = new Profitability
                    {
                        ProfitabilityPercentage = percentaje,
                        StartTime = _genericUtil.GetDateZone().AddHours(i).ToString("HH:00:00"),
                        FinalTIme = _genericUtil.GetDateZone().AddHours(i).ToString("HH:59:59"),
                        UpdatedDate = _genericUtil.GetDateZone()
                    };

                    response = await _profitabilities.Add(profitability);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", string.Join(",", ex.ToString()));
                return BadRequest(ModelState);
            }

        }

    }
}