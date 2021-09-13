using HasebCoreApi.Helpers;
using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.BankAccounts
{
    public class BankAccountsService : IBankAccountsService
    {
        private readonly IMongoRepository<BankAccount> _bankaccount;
        private readonly IMongoRepository<Branch> _branch;
        private readonly IMongoRepository<Bank> _bank;
        private readonly IMongoRepository<Currency> _currency;
        private readonly IMongoRepository<BankBranch> _bankbranch;
        private readonly IMongoRepository<InitialBankInventory> _initialBankInventory;
        //private readonly IMongoRepository<Initial> _bankbranch;
        public BankAccountsService(IMongoRepository<BankAccount> bankaccount,
            IMongoRepository<Branch> branch,
            IMongoRepository<Currency> currency,
            IMongoRepository<BankBranch> bankbranch,
            IMongoRepository<Bank> bank,
            IMongoRepository<InitialBankInventory> initialBankInventory
            )
        {
            _bankaccount = bankaccount;
            _branch = branch;
            _currency = currency;
            _bankbranch = bankbranch;
            _bank = bank;
            _initialBankInventory = initialBankInventory;
        }
        public async Task<BankAccount> Create(BankAccount bankAccount)
        {
            var branch = await _branch.FindByIdAsync(bankAccount.BranchId);
            if (branch == null)
            {
                throw new BranchNotFoundException();
            }
            bankAccount.Code = "b"+bankAccount.Code;
            await _bankaccount.InsertOneAsync(bankAccount);
            return bankAccount;
        }

        public IQueryable<object> GetBranch(string branchId)
        {
            var branch =  _branch.FindByIdAsync(branchId);
            if (branch == null) throw new BranchNotFoundException();
            //---------------------------------------------------------------
            var query = (from a in _bankaccount.AsQueryable().Where(x => x.BranchId == branchId)
                         join b in _currency.AsQueryable() on a.CurrencyId equals b.Id
                         join c in _bankbranch.AsQueryable() on a.BankBranchId equals c.Id
                         select new
                         {
                             currency = new { b.NameOne, b.Id },
                             BankBranch = new { c.BankBranchName, c.BankName, c.Id },
                             a.AccountType,
                             a.AccountNameOne,
                             a.AccountNameTwo,
                             a.CardNumber,
                             a.IBAN,
                             a.Code,
                             a.IsActive,
                             a.Id,
                         });
            return query;
        }

        public async Task<BankAccount> Get(string id)
        {
            return await _bankaccount.FindByIdAsync(id);
        }

        public async Task<BankAccount> Update(BankAccount bankAccount)
        {
            var branch = await _branch.FindByIdAsync(bankAccount.BranchId);
            if (branch == null)
            {
                throw new BranchNotFoundException();
            }
            bankAccount.Code = "b" + bankAccount.Code;
            await _bankaccount.ReplaceOneAsync(bankAccount);
            return bankAccount;
        }

        public async Task Delete(string id)
        {

            string[] keys = id.Split(",");
            foreach (var item in keys)
            {
                if (string.IsNullOrWhiteSpace(item) || item.Length != 24) throw new IdLengthNotEqual();

                var value = await _initialBankInventory.FindByIdAsync(item);

                if (value != null) throw new BankAccountReferencedException();

                await _bankaccount.DeleteByIdAsync(item);
            }

        }
    }
}

public class BankAccountReferencedException : Exception { }