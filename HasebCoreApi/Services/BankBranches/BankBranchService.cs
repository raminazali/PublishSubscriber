using HasebCoreApi.Helpers;
using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.BankBranches
{
    public class BankBranchService : IBankBranchService
    {
        private readonly IMongoRepository<BankBranch> _bankBranch;
        private readonly IMongoRepository<BankAccount> _bankAccount;
        private readonly IMongoRepository<Branch> _branch;
        public BankBranchService(IMongoRepository<BankBranch> bankBranchh, IMongoRepository<Branch> branch, IMongoRepository<BankAccount> bankAccount)
        {
            _bankBranch = bankBranchh;
            _branch = branch;
            _bankAccount = bankAccount;
        }
        public async Task<BankBranch> Create(BankBranch bankBranch)
        {
            var branch = await _branch.FindByIdAsync(bankBranch.BranchId);
            if (branch == null) throw new BranchNotFoundException();

            await _bankBranch.InsertOneAsync(bankBranch);
            return bankBranch;

        }

        public async Task Delete(string id)
        {
            var bankId = await _bankAccount.FindOneAsync(x=>x.BankBranchId == id);
            if (bankId != null) throw new BankBranchIdIsReferencedException();

            await _bankBranch.DeleteByIdAsync(id);
        }

        public async Task<BankBranch> Get(string id)
        {
            return await _bankBranch.FindByIdAsync(id);
        }

        public IEnumerable<object> GetByBranch(string branchId, string bankName)
        {
            var branch = _branch.FindOneAsync(x=>x.Id == branchId);
            if (branch == null) throw new BranchNotFoundException();
            return _bankBranch.AsQueryable().Where(x => x.BranchId == branchId && x.BankName == bankName).ToList().Select(x =>
            new { BankBranchId = x.Id, BankBranchName = x.BankBranchName, BankBranchCode = x.BankBranchCode });

        }

        public async Task<BankBranch> Update(BankBranch bankBranch)
        {
            await _bankBranch.ReplaceOneAsync(bankBranch);
            return bankBranch;
        }
    }
}
public class BankBranchIdIsReferencedException : Exception { }