using Business.Interfaces.Base;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.General
{
    public interface IUserWalletAccountTransactions : IBaseService<UserWalletAccountTransaction>
    {
        Task<IEnumerable<UserWalletAccountTransaction>> ListByUserId(string userWalletAccountId);
    }
}