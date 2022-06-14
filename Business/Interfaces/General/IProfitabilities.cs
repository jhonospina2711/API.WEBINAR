using Business.Interfaces.Base;
using Entities;
using System.Threading.Tasks;

namespace Business.Interfaces.General
{
    public interface IProfitabilities : IBaseService<Profitability>
    {
        Task<ProfitabilityResponse> Get();
    }
}