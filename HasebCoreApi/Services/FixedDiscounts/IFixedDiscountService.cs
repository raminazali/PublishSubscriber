using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.FixedDiscounts
{
    public interface IFixedDiscountService
    {
        Task<FixedDiscount> Create(FixedDiscount fixedDiscount);
        Task<FixedDiscount> Update(FixedDiscount fixedDiscount);
        Task<List<FixedDiscount>> GetBranch(string branchId);
        Task<FixedDiscount> Get(string id);
        Task Delete(string id);
    }
}
