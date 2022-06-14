using Business.Interfaces.Base;
using Entities;
using System.Threading.Tasks;

namespace Business.Interfaces.General
{
    public interface IUserWalletAccounts : IBaseService<UserWalletAccount>
    {
        Task<UserWalletAccount> GetById(string id);
        Task<IResponseService> GetByUserId(int id);
        Task<IResponseService> CreateWallet(CreateWalletModel createWalletModel);
    }
}