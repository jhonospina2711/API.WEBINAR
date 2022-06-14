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
    public class UserWalletAccounts : IUserWalletAccounts
    {
        private readonly IRepository<UserWalletAccount> _repository;
        private readonly IUnitOfWork _unit;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly GenericUtil _genericUtil = new GenericUtil();
        private readonly AppSettings _appSettings;
        private readonly IWalletAccountWords _walletAccountWords;
        private readonly IUserWalletAccountTransactions _userWalletAccountTransactions;
        private readonly IUsers _users;

        public UserWalletAccounts(IRepository<UserWalletAccount> repository,
            IResponseService responseService,
            IExceptionHandler exceptionHandler,
            IUnitOfWork unit, IOptions<AppSettings> appSettings, IWalletAccountWords walletAccountWords,
            IUserWalletAccountTransactions userWalletAccountTransactions, IUsers users)
        {
            _repository = repository;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
            _unit = unit;
            _appSettings = appSettings.Value;
            _walletAccountWords = walletAccountWords;
            _userWalletAccountTransactions = userWalletAccountTransactions;
            _users = users;
        }

        public async Task<IResponseService> List()
        {
            try
            {
                var userWalletAccountList = await _repository.List(o => o.OrderBy(x => x.Id), null);

                List<BestInvestmentUser> projectCoins = (from userWalletAccount in userWalletAccountList
                                                         select new BestInvestmentUser() { Coins = Convert.ToDecimal(Environment.GetEnvironmentVariable("InitialCoins")) - userWalletAccount.Coins, Name = userWalletAccount.User.Name+" "+ userWalletAccount.User.LastName}).ToList();
                                                    
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, projectCoins.OrderByDescending(x=>x.Coins).Take(20), projectCoins.Count());
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }
        public async Task<IResponseService> Add(UserWalletAccount entity)
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

        public async Task<IResponseService> GetByUserId(int id)
        {
            try
            {
                var userWalletAccountList = await _repository.List(o => o.OrderByDescending(x => x.Id), x => x.UserId == id);
                var userWalletAccount = userWalletAccountList.SingleOrDefault(x => x.UserId == id);
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, userWalletAccount);
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<UserWalletAccount> GetById(string id)
        {
            try
            {
                var userWalletAccountList = await _repository.List(o => o.OrderByDescending(x => x.Id), x => x.Id == id);
                var userWalletAccount = userWalletAccountList.SingleOrDefault(x => x.Id == Convert.ToString(id));
                return userWalletAccount;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IResponseService> Update(UserWalletAccount entity)
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

        public async Task<IResponseService> CreateWallet(CreateWalletModel createWalletModel)
        {
            try
            {
                decimal coins = Convert.ToDecimal(Environment.GetEnvironmentVariable("InitialCoins"));
                string id = Guid.NewGuid().ToString();
                createWalletModel.UserWalletAccount.Id = id;
                createWalletModel.UserWalletAccount.CreatedDate = _genericUtil.GetDateZone();
                createWalletModel.UserWalletAccount.UpdatedDate = _genericUtil.GetDateZone();
                createWalletModel.UserWalletAccount.Coins = 0;
                createWalletModel.UserWalletAccount.Enabled = false;
                _repository.Add(createWalletModel.UserWalletAccount);
                await _unit.Commit();
                createWalletModel.WalletAccountWord.UserWalletAccountId = id;
                await _walletAccountWords.Add(createWalletModel.WalletAccountWord);
                createWalletModel.UserWalletAccountTransaction.UserWalletAccountId = id;
                createWalletModel.UserWalletAccountTransaction.Coins = coins;
                createWalletModel.UserWalletAccountTransaction.TransactionTypeId = 1;
                createWalletModel.UserWalletAccountTransaction.Id = Guid.NewGuid().ToString();
                createWalletModel.UserWalletAccountTransaction.ProfitabilityPercentaje = 0;
                createWalletModel.UserWalletAccountTransaction.CoinsEarned = 0;
                await _userWalletAccountTransactions.Add(createWalletModel.UserWalletAccountTransaction);
                createWalletModel.UserWalletAccount.Coins = coins;
                createWalletModel.UserWalletAccount.Enabled = true;
                await this.Update(createWalletModel.UserWalletAccount);
                await _users.UpdateById(createWalletModel.UserWalletAccount.UserId);
                _unit.Dispose();

                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, createWalletModel.UserWalletAccount);
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.SetResponse(true, MessagesEnum.HttpStateBadRequest, null);
                return _responseService;
            }
        }

    }
}