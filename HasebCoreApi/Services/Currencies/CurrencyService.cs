using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.Currencies
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IMongoRepository<Currency> _currency;
        public CurrencyService(IMongoRepository<Currency> currency)
        {
            _currency = currency;
        }
        public async Task<Currency> Get(string id)
        {
            return await _currency.FindByIdAsync(id);
        }

        public async Task<List<Currency>> Get()
        {
            return await _currency.FindAll();
        }
    }
}
