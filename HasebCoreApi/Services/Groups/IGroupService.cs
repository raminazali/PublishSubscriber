using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.Groups
{
    public interface IGroupService
    {
        Task<List<Group>> Get();
        Task Create(Group group);
        Task Update(Group group);
        Task<Group> Get(string id);
    }
}