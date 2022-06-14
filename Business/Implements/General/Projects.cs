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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implements.General
{
    public class Projects : IProjects
    {
        private readonly IRepository<Project> _repository;
        private readonly IUnitOfWork _unit;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly GenericUtil _genericUtil = new GenericUtil();
        private readonly AppSettings _appSettings;

        public Projects(IRepository<Project> repository,
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
                var projectList = await _repository.List(o => o.OrderBy(x => x.Id), x => x.Enabled);
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, projectList, projectList.Count());
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }
        public async Task<IResponseService> Add(Project entity)
        {
            try
            {
                _repository.Add(entity);
                await _unit.Commit();
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, "Ok");
                _unit.Dispose();
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
                var projectList = await _repository.List(o => o.OrderByDescending(x => x.Id), x => x.Id == id);
                var project = projectList.SingleOrDefault(x => x.Id == id);

                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, project);
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<IResponseService> Update(Project entity)
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
    }
}