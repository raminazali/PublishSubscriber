using HasebCoreApi.Helpers;
using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.Poses
{
    public class PosService : IPosService
    {
        private readonly IMongoRepository<Pos> _pos;
        private readonly IMongoRepository<BankAccount> _bankAccount;
        private readonly IMongoRepository<Currency> _currency;
        private readonly IMongoRepository<Branch> _branch;
        private readonly IMongoRepository<InitialPosInventory> _initialPosInventory;

        public PosService(IMongoRepository<Pos> pos,
            IMongoRepository<Currency> currency,
            IMongoRepository<BankAccount> bankAccount,
            IMongoRepository<Branch> branch,
            IMongoRepository<InitialPosInventory> initialPosInventory
            )
        {
            _pos = pos;
            _currency = currency;
            _bankAccount = bankAccount;
            _branch = branch;
            _initialPosInventory = initialPosInventory;
        }
        public async Task<Pos> Create(Pos pos)
        {
            var branch = await _branch.FindByIdAsync(pos.BranchId);
            if (branch == null) throw new BranchNotFoundException();
            var currency = await _currency.FindByIdAsync(pos.CurrencyId);
            if (currency == null) throw new CurrencyNotFoundException();
            var bankAccount = await _bankAccount.FindByIdAsync(pos.BankAccountId);
            if (bankAccount == null) throw new BankAccountNotFoundException();

            pos.Code = "k" + pos.Code;
            await _pos.InsertOneAsync(pos);
            return pos;
        }

        public async Task Delete(string id)
        {
            string[] keys = id.Split(",");
            foreach (string item in keys)
            {
                if (string.IsNullOrWhiteSpace(item) || item.Length != 24) throw new IdLengthNotEqual();

                var value = await _initialPosInventory.FindByIdAsync(item);
                if (value != null) throw new ReferencedToInitialPosException();

                await _pos.DeleteByIdAsync(item);
            }
        }

        public async Task<Pos> Get(string id)
        {
            return await _pos.FindByIdAsync(id);
        }

        public IQueryable<object> GetBranch(string branchId)
        {
            var branch = _branch.FindByIdAsync(branchId);
            if (branch == null) throw new BranchNotFoundException();

            var query = (from a in _pos.AsQueryable()
                         join b in _currency.AsQueryable() on a.CurrencyId equals b.Id
                         join c in _bankAccount.AsQueryable() on a.BankAccountId equals c.Id
                         where a.BranchId == branchId
                         select new
                         {
                             a.Id,
                             a.PosNameOne,
                             a.PosNameTwo,
                             a.IsActive,
                             a.TerminalNumber,
                             a.Code,
                             BankAccountName = c.AccountNameOne,
                             BankAccountId = c.Id,
                             CurrencyName = b.NameOne,
                             CurrencyId = b.Id
                         });
            return query;
        }

        public async Task<Pos> Update(Pos pos)
        {
            pos.Code = "k" + pos.Code;
            await _pos.ReplaceOneAsync(pos);
            return pos;
        }
    }
}
public class CurrencyNotFoundException : Exception { }
public class BankAccountNotFoundException : Exception { }
public class ReferencedToInitialPosException : Exception { }
