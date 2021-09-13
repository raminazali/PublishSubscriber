using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.Commissions
{
    public class CommissionService : ICommissionService
    {
        private readonly IMongoRepository<CommissionTbl> _commission;
        private readonly IMongoRepository<Branch> _branch;
        public CommissionService(IMongoRepository<CommissionTbl> commission, IMongoRepository<Branch> branch)
        {
            _commission = commission;
            _branch = branch;
        }
        public async Task<CommissionTbl> Create(CommissionTbl commission)
        {
            var branch = await _branch.FindByIdAsync(commission.BranchId);
            if (branch == null) throw new BranchNotFoundException();

            await _commission.InsertOneAsync(commission);
            return commission;
        }

        public Task<CommissionTbl> Get(string id)
        {
            return _commission.FindByIdAsync(id);
        }

        public IQueryable<object> GetBranch(string branchId)
        {
            var branch = _branch.FindByIdAsync(branchId);
            if (branch == null) throw new BranchNotFoundException();

            return _commission.AsQueryable().Where(x => x.BranchId == branchId);
        }

        public Task<CommissionTbl> Update(CommissionTbl commission)
        {
            throw new NotImplementedException();
        }
    }
}
