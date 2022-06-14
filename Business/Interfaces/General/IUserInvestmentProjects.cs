using Business.Interfaces.Base;
using Entities;
using System.Threading.Tasks;

namespace Business.Interfaces.General
{
    public interface IUserInvestmentProjects : IBaseService<UserInvestmentProject>
    {
        Task<IResponseService> CreateInvestmentProject(UserInvestmentProjectModel createInvestmentProject);
        Task<IResponseService> GetByProjectId(int projectId);
        Task<UserInvestmentProject> GetByTransactionId(string transactionId);
        Task<IResponseService> GetUserCoinsWinsByUserId(string userWalletAccountId);
    }
}