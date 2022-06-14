using Business.Interfaces.Base;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.General
{
    public interface IUsersOneTimePass : IBaseService<UserOneTimePass>
    {
        Task<IResponseService> CreateOTP(string email, int userId, bool isForgotPassword);
        Task<IResponseService> ValidateOTP(int userId, int codeValidate);
        Task<IResponseService> CreateSupportOTP(string email, int userId, List<string> emails);
    }
}