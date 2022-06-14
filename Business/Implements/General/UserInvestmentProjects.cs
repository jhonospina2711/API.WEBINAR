using Business.Interfaces.Base;
using Business.Interfaces.General;
using Data.Interfaces;
using Entities;
using Entities.Enums;
using Entities.Helpers;
using Entities.Utils;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Implements.General
{
    public class UserInvestmentProjects : IUserInvestmentProjects
    {
        private readonly IRepository<UserInvestmentProject> _repository;
        private readonly IUnitOfWork _unit;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly GenericUtil _genericUtil = new GenericUtil();
        private readonly AppSettings _appSettings;
        private readonly IUserWalletAccountTransactions _userWalletAccountTransactions;
        private readonly IUserWalletAccounts _userWalletAccounts;
        private readonly IProfitabilities _profitabilities;

        public UserInvestmentProjects(IRepository<UserInvestmentProject> repository,
            IResponseService responseService,
            IExceptionHandler exceptionHandler,
            IUnitOfWork unit, IOptions<AppSettings> appSettings,
            IUserWalletAccountTransactions userWalletAccountTransactions, IUserWalletAccounts userWalletAccounts,
            IProfitabilities profitabilities)
        {
            _repository = repository;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
            _unit = unit;
            _appSettings = appSettings.Value;
            _userWalletAccountTransactions = userWalletAccountTransactions;
            _userWalletAccounts = userWalletAccounts;
            _profitabilities = profitabilities;
        }

        public async Task<IResponseService> List()
        {
            try
            {
                var UserInvestmentProjectList = await _repository.List(o => o.OrderBy(x => x.Id), null);

                List<BestInvestmentUser> projectCoins = (from userInvestmentProject in UserInvestmentProjectList
                                              select new BestInvestmentUser() { Name = userInvestmentProject .Project.Name, Coins = userInvestmentProject .UserWalletAccountTransaction.Coins}).ToList();

                List<BestInvestmentUser> resultProjectsCoins = projectCoins.GroupBy(l => l.Name).Select(cl => new BestInvestmentUser { Name = cl.First().Name, Coins = cl.Sum(c => c.Coins) }).ToList();

                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, resultProjectsCoins.OrderByDescending(x=>x.Coins).Take(20), resultProjectsCoins.Count());
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }
        public async Task<IResponseService> Add(UserInvestmentProject entity)
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
                var userInvestmentProjectList = await _repository.List(o => o.OrderByDescending(x => x.Id), x => x.Id == id);
                var userInvestmentProject = userInvestmentProjectList.SingleOrDefault(x => x.Id == id);

                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, userInvestmentProject);
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<IResponseService> Update(UserInvestmentProject entity)
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

        public async Task<IResponseService> CreateInvestmentProject(UserInvestmentProjectModel createInvestmentProject)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                var profitability = _profitabilities.Get();
                UserWalletAccountTransaction userWalletAccountTransaction = new UserWalletAccountTransaction
                {
                    Id = id,
                    Coins = createInvestmentProject.Coins,
                    UserWalletAccountId = createInvestmentProject.UserWalletAccountAccountId,
                    ProfitabilityPercentaje = profitability.Result.ProfitabilityPercentage,
                    CoinsEarned = ((createInvestmentProject.Coins * profitability.Result.ProfitabilityPercentage) / 100),
                    TransactionTypeId = 2,
                };

                await _userWalletAccountTransactions.Add(userWalletAccountTransaction);

                UserInvestmentProject userInvestmentProject = new UserInvestmentProject
                {
                    UserWalletAccountTransactionId = id,
                    ProjectId = createInvestmentProject.ProjectId,
                    CreatedDate = _genericUtil.GetDateZone()
                };
                _repository.Add(userInvestmentProject);
                await _unit.Commit();
                UserWalletAccount userWalletAccount = await _userWalletAccounts.GetById(createInvestmentProject.UserWalletAccountAccountId);
                userWalletAccount.Coins -= createInvestmentProject.Coins;
                userWalletAccount.UpdatedDate = _genericUtil.GetDateZone();
                await _userWalletAccounts.Update(userWalletAccount);
                _unit.Dispose();

                UserInvestmentProjectResponse userInvestmentProjectResponse = new UserInvestmentProjectResponse
                {
                    UserWalletAccountTransactionId = id,
                    Coins = userWalletAccount.Coins,
                    ProjectCoins = createInvestmentProject.Coins,
                    ProfitabilityPercentaje = profitability.Result.ProfitabilityPercentage
                };
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, userInvestmentProjectResponse);
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.SetResponse(true, MessagesEnum.HttpStateBadRequest, null);
                return _responseService;
            }
        }

        public async Task<IResponseService> GetByProjectId(int projectId)
        {
            try
            {
                var userInvestmentProjectList = await _repository.List(o => o.OrderByDescending(x => x.Id), x => x.ProjectId == projectId);
                List<decimal> projectCoins = (from userInvestmentProject in userInvestmentProjectList
                                              select userInvestmentProjectList.Where(x => x.UserWalletAccountTransactionId == userInvestmentProject.UserWalletAccountTransactionId).FirstOrDefault().UserWalletAccountTransaction.Coins).ToList();

                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, projectCoins.Sum());
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<UserInvestmentProject> GetByTransactionId(string transactionId)
        {
            try
            {
                var userInvestmentProjectList = await _repository.List(o => o.OrderByDescending(x => x.Id), x => x.UserWalletAccountTransactionId == transactionId);
                return  userInvestmentProjectList.SingleOrDefault(x => x.UserWalletAccountTransactionId == transactionId);

            }catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<IResponseService> GetUserCoinsWinsByUserId(string userWalletAccountId)
        {
            try
            {
                var userWalletAccountTransactionList = await _userWalletAccountTransactions.ListByUserId(userWalletAccountId);

                List<UserWalletAccountTransactionResponse> userWalletAccountTransactionResponses = new List<UserWalletAccountTransactionResponse>();
                UserWalletAccountTransactionResponse userWalletAccountTransactionResponse;
                foreach (var userAccountTransaction in userWalletAccountTransactionList)
                {
                    if (userAccountTransaction.TransactionTypeId == 2)
                    {
                        userWalletAccountTransactionResponse = new UserWalletAccountTransactionResponse
                        {
                            Id = userAccountTransaction.Id,
                            ProfitabilityPercentaje = userAccountTransaction.ProfitabilityPercentaje,
                            Coins = "-" + Convert.ToString(Convert.ToInt32(userAccountTransaction.Coins)),
                            CoinsEarned = userAccountTransaction.CoinsEarned,
                            CreatedDate = userAccountTransaction.CreatedDate.ToString("MMM yy"),
                            TransactionTypeId = userAccountTransaction.TransactionTypeId,
                            UserWalletAccountId = userAccountTransaction.UserWalletAccountId,
                            ProjectName = this.GetByTransactionId(userAccountTransaction.Id).Result.Project.Name
                        };
                    }
                    else
                    {
                        userWalletAccountTransactionResponse = new UserWalletAccountTransactionResponse
                        {
                            Id = userAccountTransaction.Id,
                            ProfitabilityPercentaje = userAccountTransaction.ProfitabilityPercentaje,
                            Coins = "+" + Convert.ToString(Convert.ToInt32(userAccountTransaction.Coins)),
                            CoinsEarned = userAccountTransaction.CoinsEarned,
                            CreatedDate = userAccountTransaction.CreatedDate.ToString("MMM yy"),
                            TransactionTypeId = userAccountTransaction.TransactionTypeId,
                            UserWalletAccountId = userAccountTransaction.UserWalletAccountId,
                            ProjectName = ""
                        };
                    }

                    userWalletAccountTransactionResponses.Add(userWalletAccountTransactionResponse);
                }

                _responseService.SetResponseDecimal(true, MessagesEnum.HttpStateOk, userWalletAccountTransactionResponses, userWalletAccountTransactionResponses.Sum(x => x.CoinsEarned));
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