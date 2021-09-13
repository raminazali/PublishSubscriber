using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.Currencies
{
    public interface ICurrencyService
    {
        Task<Currency> Get(string id);
        Task<List<Currency>> Get();
    }
}
