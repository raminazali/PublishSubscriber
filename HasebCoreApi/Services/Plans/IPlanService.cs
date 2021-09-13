using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.Plans
{
    public interface IPlanService
    {
        Task Create(Plan plan);
        Task Update(Plan plan);
        Task<List<Plan>> Get();
        Task<Plan> Get(string id);
    }
}