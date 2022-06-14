using Business.Interfaces.Base;
using Business.Interfaces.General;
using Data.Interfaces;
using Entities;
using Entities.Enums;
using Entities.Helpers;
using Entities.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Rest;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implements.General
{
    public class Profitabilities : IProfitabilities
    {
        private readonly IRepository<Profitability> _repository;
        private readonly IUnitOfWork _unit;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly GenericUtil _genericUtil = new GenericUtil();
        private readonly AppSettings _appSettings;

        public Profitabilities(IRepository<Profitability> repository,
            IResponseService responseService,
            IExceptionHandler exceptionHandler,
            IUnitOfWork unit, IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
            _unit = unit;
            _appSettings = appSettings.Value;
        }

        public async Task<IResponseService> List()
        {
            try
            {
                var profitabilityList = await _repository.List(o => o.OrderBy(x => x.Id),null);
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, profitabilityList, profitabilityList.Count());
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }
        public async Task<IResponseService> Add(Profitability entity)
        {
            try
            {
                _repository.Add(entity);
                await _unit.Commit();
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, "Ok");
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.SetResponse(true, MessagesEnum.HttpStateBadRequest, null);
                return _responseService;
            }
        }

        public async Task<IResponseService> Delete(int id)
        {
            try
            {
                await _repository.Delete(id);
                await _unit.Commit();
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, "Ok");
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<IResponseService> Get(int id)
        {
            try
            {
                var profitabilityList = await _repository.List(o => o.OrderBy(x => x.Id), null);
                List<ProfitabilityResponse> ProfitabilityResponseList = new List<ProfitabilityResponse>();
                ProfitabilityResponse profitabilityResponse;
                foreach (var profitability in profitabilityList)
                {
                    profitabilityResponse = new ProfitabilityResponse
                    {
                        Id = profitability.Id,
                        ProfitabilityPercentage = profitability.ProfitabilityPercentage,
                        StartTime = Convert.ToDateTime(_genericUtil.GetDateZone().ToString("dd/MM/yyyy") + " " + profitability.StartTime),
                        FinalTIme = Convert.ToDateTime(_genericUtil.GetDateZone().ToString("dd/MM/yyyy") + " " + profitability.FinalTIme),
                        UpdatedDate = profitability.UpdatedDate
                    };
                    ProfitabilityResponseList.Add(profitabilityResponse);
                }

                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, ProfitabilityResponseList.SingleOrDefault(x => x.StartTime <= _genericUtil.GetDateZone() && x.FinalTIme >= _genericUtil.GetDateZone()));
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<IResponseService> Update(Profitability entity)
        {
            try
            {
                _repository.Update(entity);
                await _unit.Commit();
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, "Ok");
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<ProfitabilityResponse> Get()
        {
            try
            {
                var profitabilityList = await _repository.List(o => o.OrderByDescending(x => x.Id),null);
                List<ProfitabilityResponse> ProfitabilityResponseList = new List<ProfitabilityResponse>();
                ProfitabilityResponse profitabilityResponse;
                foreach (var profitability in profitabilityList)
                {
                    profitabilityResponse = new ProfitabilityResponse
                    {
                        Id = profitability.Id,
                        ProfitabilityPercentage = profitability.ProfitabilityPercentage,
                        StartTime = Convert.ToDateTime(_genericUtil.GetDateZone().ToString("dd/MM/yyyy") + " " + profitability.StartTime),
                        FinalTIme = Convert.ToDateTime(_genericUtil.GetDateZone().ToString("dd/MM/yyyy") + " " + profitability.FinalTIme),
                        UpdatedDate = profitability.UpdatedDate
                    };
                    ProfitabilityResponseList.Add(profitabilityResponse);
                }

                return ProfitabilityResponseList.SingleOrDefault(x => x.StartTime <= _genericUtil.GetDateZone() && x.FinalTIme >= _genericUtil.GetDateZone());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}