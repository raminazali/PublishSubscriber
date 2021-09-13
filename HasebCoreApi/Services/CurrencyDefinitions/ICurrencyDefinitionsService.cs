using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.CurrencyDefinitions
{
    public interface ICurrencyDefinitionsService
    {
        Task<CurrencyDefinition> Create(CurrencyDefinition currencyDefinition);
        Task<CurrencyDefinition> Update(CurrencyDefinition currencyDefinition);
        //bool GetBase(string branchId);
        Task<CurrencyDefinition> Get(string id);
        IQueryable<object> GetWithbranch(string  branchId);
        Task Delete(string id);

    }
}
