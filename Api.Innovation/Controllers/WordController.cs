using Entities;
using Business.Interfaces.General;
using Entities.Enums;
using Entities.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using Business.Interfaces.Base;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Api.Covid19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IResponseService _responseService;
        public WordController(IResponseService responseService)
        {
            _responseService = responseService;
        }

        [Authorize]
        [HttpGet("Listwords")]
        public IActionResult Listwords()
        {
            try
            {
                StreamReader r = new StreamReader("./words.json");
                var Words = JsonConvert.DeserializeObject<List<string>>(r.ReadToEnd());
                var WordsList = Words.OrderBy(q => Guid.NewGuid()).Take(12);
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, WordsList);
                return Ok(_responseService);
                
            }catch(Exception ex)
            {
                ModelState.AddModelError("Error", string.Join(",", ex.ToString()));
                return BadRequest(ModelState);
            }

        }

    }
}