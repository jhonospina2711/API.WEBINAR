using Business.Interfaces.Base;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.General
{
    public interface IUserProfiles : IBaseService<UserProfile>
    {
        Task<List<string>> ListByProflieId();
    }
}