using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Http;

namespace HasebCoreApi.Services.Commodities
{
    public interface ICommodityService
    {
        Task<Commodity> Create(Commodity commodity, List<IFormFile> files);

        Task<Commodity> Update(Commodity commodity, List<IFormFile> files);

        IQueryable<object> GetAll(string branchId);
        Task<List<Commodity>> GetbyCommo(bool IsCommodity, string commodityGroupId);

        IQueryable<object> Get(string branchId, bool? IsCommodity);

        Task<string> DeleteFile(string FileNameAndPath);
        Task Delete(string id);

        Task<object> Get(string id);

        Task<Commodity> GetById(string id);

        // IEnumerable<object> GetDiscountByPeriod(string BranchId, string PeriodId, bool IsCommodity);

    }
}