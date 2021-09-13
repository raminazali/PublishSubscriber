using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.Periods
{
    public interface IPeriodService
    {
        Task<Period> Create(Period pricePeriod);
        Task<Period> CreateDiscount(Period discountPeriod);
        Task<Period> Createcommission(Period commissionPeriod);
        Task<object> GetCommodityPricePeriods(string branchId, string commodityId);
        Task<object> GetByType(string branchId, string type, int reference,bool Isbuy);
        IEnumerable<object> GetQueryable(string BranchId, int reference, bool Isbuy);
    }
}
