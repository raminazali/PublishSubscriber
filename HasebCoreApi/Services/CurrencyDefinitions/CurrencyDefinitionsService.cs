using HasebCoreApi.Helpers;
using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.CurrencyDefinitions
{
    public class CurrencyDefinitionsService : ICurrencyDefinitionsService
    {
        private readonly IMongoRepository<CurrencyDefinition> _currencyDefinitions;
        private readonly IMongoRepository<Branch> _branch;
        private readonly IMongoRepository<Currency> _currency;
        public CurrencyDefinitionsService(IMongoRepository<CurrencyDefinition> currencyDefinitions, IMongoRepository<Branch> branch, IMongoRepository<Currency> currency)
        {
            _currencyDefinitions = currencyDefinitions;
            _branch = branch;
            _currency = currency;
        }
        public async Task<CurrencyDefinition> Create(CurrencyDefinition currencyDefinition)
        {
            await _currencyDefinitions.InsertOneAsync(currencyDefinition);
            return currencyDefinition;
        }

        public async Task Delete(string id)
        {

            string[] keys = id.Split(",");
            foreach (string item in keys)
            {
                if (string.IsNullOrWhiteSpace(item) || item.Length != 24) throw new IdLengthNotEqual();

                await _currencyDefinitions.DeleteByIdAsync(item);
            }
        }

        public async Task<CurrencyDefinition> Get(string id)
        {
            return await _currencyDefinitions.FindByIdAsync(id);
        }

        public IQueryable<object> GetWithbranch(string branchId)
        {
            var branch = _branch.FindOneAsync(x => x.Id == branchId);
            if (branch == null) throw new BranchNotFoundException();

            var query = (from a in _currencyDefinitions.AsQueryable().Where(x => x.BranchId == branchId)
                         join b in _currency.AsQueryable() on a.CurrencyId equals b.Id
                         select new
                         {
                             a.BaseRate,
                             a.CreateDate,
                             a.IsActive,
                             a.Id,
                             a.CurrencyId,
                             b.NameOne,
                             b.NameTwo,
                             b.Abbreviation,
                             b.ForeignExchangeCode

                         });
            return query;
        }

        public async Task<CurrencyDefinition> Update(CurrencyDefinition currencyDefinition)
        {
            await _currencyDefinitions.ReplaceOneAsync(currencyDefinition);
            return currencyDefinition;
        }

    }
}
