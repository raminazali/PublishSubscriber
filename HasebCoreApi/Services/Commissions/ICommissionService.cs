using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.Commissions
{
    public interface ICommissionService
    {
        IQueryable<object> GetBranch(string branchId);
        Task<CommissionTbl> Get(string id);
        Task<CommissionTbl> Create(CommissionTbl commission);
        Task<CommissionTbl> Update(CommissionTbl commission);

    }
}
