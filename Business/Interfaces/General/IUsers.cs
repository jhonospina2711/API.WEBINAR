using Business.Interfaces.Base;
using Entities;
using System.Threading.Tasks;

namespace Business.Interfaces.General
{
    public interface IUsers : IBaseService<User>
    {
        Task<IResponseService> ValidateUserActivate(string email);
        Task<IResponseService> CreatePassword(SetPasswordModel setPasswordModel);
        Task<IResponseService> Authenticate(string email, string password);
        Task<IResponseService> UpdateById(int id);
        Task<IResponseService> ForgotUserpassword(string email);
        Task<IResponseService> SendCodeSupport(string email);
    }
}