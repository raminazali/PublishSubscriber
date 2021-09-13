using HasebCoreApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.CommodityGroups
{
    public interface ICommodityGroupService
    {
        Task Create(CommodityGroup commodity);
        Task Update(CommodityGroup commodity);
        Task<List<CommodityGroup>> Get();
        Task<CommodityGroup> Get(string id);
        Task<List<CommodityGroup>> GetByParent(string subToId, string branchId);
        Task Delete(string id);
    }
}