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
    public class Users : IUsers
    {
        private readonly IRepository<User> _repository;
        private readonly IUnitOfWork _unit;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly GenericUtil _genericUtil = new GenericUtil();
        private readonly AppSettings _appSettings;
        private readonly IUsersOneTimePass _usersOneTimePass;
        private readonly IUserProfiles _userProfiles;

        public Users(IRepository<User> repository,
            IResponseService responseService,
            IExceptionHandler exceptionHandler,
            IUnitOfWork unit, IOptions<AppSettings> appSettings, IUsersOneTimePass usersOneTimePass, IUserProfiles userProfiles)
        {
            _repository = repository;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
            _unit = unit;
            _appSettings = appSettings.Value;
            _usersOneTimePass = usersOneTimePass;
            _userProfiles = userProfiles;
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
        public async Task<IResponseService> Add(User entity)
        {
            try
            {
                if (await _repository.Any(x => x.Email == entity.Email)) _responseService.Meta.Errors.Add(MessagesEnum.EmailAlreadyUsed);

                if (_responseService.Meta.Errors.Count == 0)
                {
                    entity.Password = Encrypter.EncryptString(entity.Password, _appSettings.Secret);
                    _repository.Add(entity);
                    await _unit.Commit();
                    _responseService.SetResponse(true, MessagesEnum.HttpStateOk, "Ok");
                    _unit.Dispose();
                    return _responseService;
                }
                else
                {
                    _responseService.SetResponse(true, MessagesEnum.HttpStateBadRequest, null);
                    return _responseService;
                }
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
                var userList = await _repository.List(o => o.OrderByDescending(x => x.CreatedDate), x => x.Id == id);
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

        public async Task<IResponseService> Update(User entity)
        {
            try
            {
                entity.UpdatedDate = _genericUtil.GetDateZone();
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



        public async Task<IResponseService> Authenticate(string email, string password)
        {
            try
            {
                AuthenticationResponse authResponse = new AuthenticationResponse();
                var encryptedPassword = Encrypter.EncryptString(password, _appSettings.Secret);
                var userList = await _repository.List(o => o.OrderByDescending(x => x.CreatedDate), x => x.Email == email);
                var user = userList.SingleOrDefault(x => x.Email == email && x.Password == encryptedPassword && x.Enabled);

                if (user == null)
                {
                    authResponse.Authenticated = false;
                    authResponse.Token = null;
                    authResponse.UserId = 0;
                    authResponse.IsFirstLogin = true;
                    authResponse.InitialCoins = 0;
                }
                else
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Email.ToString())
                }),
                        Expires = DateTime.Now.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    authResponse.Authenticated = true;
                    authResponse.Token = tokenHandler.WriteToken(token);
                    authResponse.UserId = user.Id;
                    authResponse.IsFirstLogin = user.IsFirstLogin;
                    authResponse.InitialCoins = Convert.ToDecimal(Environment.GetEnvironmentVariable("InitialCoins"));
                }

                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, authResponse);
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.SetResponse(true, MessagesEnum.HttpStateBadRequest, null);
                return _responseService;
            }
        }

        public async Task<IResponseService> CreatePassword(SetPasswordModel setPasswordModel)
        {
            try
            {
                var userList = await _repository.List(o => o.OrderByDescending(x => x.CreatedDate), x => x.Id == setPasswordModel.UserId);
                var user = userList.SingleOrDefault(x => x.Id == setPasswordModel.UserId && x.Enabled);

                if (user == null)
                {
                    _responseService.Meta.Errors.Add(MessagesEnum.EmailError);
                    _responseService.SetResponse(true, MessagesEnum.HttpStateOk);
                    return _responseService;
                }

                var encryptedPassword = Encrypter.EncryptString(setPasswordModel.Password, _appSettings.Secret);
                user.Password = encryptedPassword;
                user.UpdatedDate = _genericUtil.GetDateZone();
                user.Activated = true;
                _repository.Update(user);
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

        public async Task<IResponseService> ValidateUserActivate(string email)
        {
            try
            {
                var userList = await _repository.List(o => o.OrderByDescending(x => x.CreatedDate), x => x.Email == email);
                var user = userList.SingleOrDefault(x => x.Email == email && x.Enabled);
                UserActivate userActivate = new UserActivate();

                if (user == null)
                {
                    _responseService.Meta.Errors.Add(MessagesEnum.EmailError);
                    _responseService.SetResponse(true, MessagesEnum.HttpStateOk, null);
                    return _responseService;
                }
                else if (!user.Activated)
                    await _usersOneTimePass.CreateOTP(user.Email, user.Id, false);

                userActivate.Id = user.Id;
                userActivate.Email = user.Email;
                userActivate.Activate = user.Activated;

                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, userActivate);
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<IResponseService> UpdateById(int id)
        {
            try
            {
                var userList = await _repository.List(o => o.OrderByDescending(x => x.CreatedDate), x => x.Id == id);
                var entity = userList.SingleOrDefault(x => x.Id == id);
                entity.IsFirstLogin = false;
                await this.Update(entity);
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

        public async Task<IResponseService> ForgotUserpassword(string email)
        {
            try
            {
                var userList = await _repository.List(o => o.OrderByDescending(x => x.CreatedDate), x => x.Email == email);
                var user = userList.SingleOrDefault(x => x.Email == email && x.Enabled);
                UserActivate userActivate = new UserActivate();

                if (user == null)
                {
                    _responseService.Meta.Errors.Add(MessagesEnum.EmailError);
                    _responseService.SetResponse(true, MessagesEnum.HttpStateOk, null);
                    return _responseService;
                }
                await _usersOneTimePass.CreateOTP(user.Email, user.Id, true);

                user.Activated = false;
                user.Password = null;
                await this.Update(user);
                userActivate.Id = user.Id;
                userActivate.Email = user.Email;
                userActivate.Activate = user.Activated;

                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, userActivate);
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }
        public async Task<IResponseService> SendCodeSupport(string email)
        {
            try
            {
                var userList = await _repository.List(o => o.OrderByDescending(x => x.CreatedDate), x => x.Email == email);
                var user = userList.SingleOrDefault(x => x.Email == email && x.Enabled);
                var emails =  await _userProfiles.ListByProflieId();
                await _usersOneTimePass.CreateSupportOTP(user.Email, user.Id, emails);
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, true);
                return _responseService;
            }
            catch(Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }


    }
}