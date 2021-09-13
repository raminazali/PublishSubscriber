using HasebCoreApi.Helpers;
using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.FixedDiscounts
{
    public class FixedDiscountService : IFixedDiscountService
    {
        private readonly IMongoRepository<FixedDiscount> _fixedDiscount;
        private readonly IMongoRepository<Branch> _branch;
        public FixedDiscountService(IMongoRepository<FixedDiscount> fixedDiscount, IMongoRepository<Branch> branch)
        {
            _fixedDiscount = fixedDiscount;
            _branch = branch;
        }
        public async Task<FixedDiscount> Create(FixedDiscount fixedDiscount)
        {
            var branch = await _branch.FindByIdAsync(fixedDiscount.BranchId);
            if (branch == null)
            {
                throw new BranchNotFoundException();
            }
            await _fixedDiscount.InsertOneAsync(fixedDiscount);
            return fixedDiscount;
        }

        public async Task<List<FixedDiscount>> GetBranch(string branchId)
        {
            var branch = await _branch.FindByIdAsync(branchId);
            if (branch == null) throw new BranchNotFoundException();
            return await _fixedDiscount.AsQueryable().Where(x => x.BranchId == branchId).ToListAsyncSafe();
        }

        public async Task<FixedDiscount> Get(string id)
        {
            return await _fixedDiscount.FindByIdAsync(id);
        }

        public async Task<FixedDiscount> Update(FixedDiscount fixedDiscount)
        {

            await _fixedDiscount.ReplaceOneAsync(fixedDiscount);
            return fixedDiscount;
        }

        public async Task Delete(string id)
        {
            string[] keys = id.Split(",");
            foreach (string item in keys)
            {
                if (string.IsNullOrWhiteSpace(item) || item.Length != 24) throw new IdLengthNotEqual();

                await _fixedDiscount.DeleteByIdAsync(item);
            }
        }
    }
}
