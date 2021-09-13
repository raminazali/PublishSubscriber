using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.Banks
{
    public interface IBankService
    {
        Task<List<Bank>> Get();
        Task<Bank> Get(string id);
        Task<Bank> Create(Bank bank);
    }
}
