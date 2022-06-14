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
    public class UserWalletAccountTransactions : IUserWalletAccountTransactions
    {
        private readonly IRepository<UserWalletAccountTransaction> _repository;
        private readonly IUnitOfWork _unit;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly GenericUtil _genericUtil = new GenericUtil();
        private readonly AppSettings _appSettings;

        public UserWalletAccountTransactions(IRepository<UserWalletAccountTransaction> repository,
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
                var UserWalletAccountTransactionList = await _repository.List(o => o.OrderBy(x => x.Id), null);
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, UserWalletAccountTransactionList, UserWalletAccountTransactionList.Count());
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }
        public async Task<IResponseService> Add(UserWalletAccountTransaction entity)
        {
            try
            {
                entity.CreatedDate = _genericUtil.GetDateZone();
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
                var userList = await _repository.List(o => o.OrderByDescending(x => x.Id), x => x.Id == Convert.ToString(id));
                var user = userList.SingleOrDefault(x => x.Id == Convert.ToString(id));

                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, user);
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<IResponseService> Update(UserWalletAccountTransaction entity)
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

        public async Task<IEnumerable<UserWalletAccountTransaction>> ListByUserId(string userWalletAccountId)
        {
            try
            {
                var userWalletAccountTransactionList = await _repository.List(o => o.OrderByDescending(x => x.CreatedDate), x => x.UserWalletAccountId == userWalletAccountId);

                return userWalletAccountTransactionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}