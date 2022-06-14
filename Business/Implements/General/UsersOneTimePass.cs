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
    public class UsersOneTimePass : IUsersOneTimePass
    {
        private readonly IRepository<UserOneTimePass> _repository;
        private readonly IUnitOfWork _unit;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly GenericUtil _genericUtil = new GenericUtil();
        private readonly AppSettings _appSettings;

        public UsersOneTimePass(IRepository<UserOneTimePass> repository,
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
                var UserList = await _repository.List(o => o.OrderBy(x => x.Id), null);
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, UserList, UserList.Count());
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }
        public async Task<IResponseService> Add(UserOneTimePass entity)
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
                var userList = await _repository.List(o => o.OrderByDescending(x => x.Id), x => x.Id == id);
                var user = userList.SingleOrDefault(x => x.Id == id);

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

        public async Task<IResponseService> Update(UserOneTimePass entity)
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


        public async Task<IResponseService> CreateOTP(string email, int userId, bool isForgotPassword)
        {
            try
            {
                UserOneTimePass userOneTimePass = new UserOneTimePass();
                int codeValidation = new Random().Next(1000, 9999);
                if (isForgotPassword)
                {
                    GenericUtil.SendOTPResetPasswordMessage(email, codeValidation);
                    userOneTimePass.ReceivedByUser = true;
                }
                else
                {
                    GenericUtil.SendOTPMessage(email, codeValidation);
                    userOneTimePass.ReceivedByUser = true;

                }

                userOneTimePass.Used = false;
                userOneTimePass.UserId = userId;
                userOneTimePass.OTPCode = codeValidation;
                userOneTimePass.CreatedDate = _genericUtil.GetDateZone();

                _repository.Add(userOneTimePass);
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

        public async Task<IResponseService> ValidateOTP(int userId, int codeValidate)
        {
            try
            {
                var oTPList = await _repository.List(o => o.OrderByDescending(x => x.CreatedDate), x => x.UserId == userId && x.OTPCode == codeValidate && !x.Used);
                var oTP = oTPList.SingleOrDefault(x => x.UserId == userId && x.OTPCode == codeValidate);
                if (oTP == null)
                {
                    _responseService.Meta.Errors.Add(MessagesEnum.OtpError);
                    _responseService.SetResponse(true, MessagesEnum.HttpStateOk, null);
                    return _responseService;
                }
                else if ((_genericUtil.GetDateZone() - oTP.CreatedDate).Minutes < 3)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, userId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, codeValidate.ToString())
                    }),
                        Expires = DateTime.Now.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenValue = tokenHandler.WriteToken(token);
                    oTP.Used = true;
                    _repository.Update(oTP);
                    await _unit.Commit();
                    _responseService.SetResponse(true, MessagesEnum.HttpStateOk, tokenValue);
                    return _responseService;
                }
                else
                {
                    _responseService.Meta.Errors.Add(MessagesEnum.OtpTimeError);
                    _responseService.SetResponse(true, MessagesEnum.HttpStateOk, null);
                    return _responseService;
                }
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<IResponseService> CreateSupportOTP(string email, int userId, List<string> emails)
        {
            try
            {
                UserOneTimePass userOneTimePass = new UserOneTimePass();
                int codeValidation = new Random().Next(1000, 9999);

                GenericUtil.SendOTPSupport(email, codeValidation,emails);
                userOneTimePass.ReceivedByUser = true;

                userOneTimePass.ReceivedByUser = false;
                userOneTimePass.Used = false;
                userOneTimePass.UserId = userId;
                userOneTimePass.OTPCode = codeValidation;
                userOneTimePass.CreatedDate = _genericUtil.GetDateZone();

                _repository.Add(userOneTimePass);
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
    }
}