using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.Banks
{
    public class BankService : IBankService
    {
        private readonly IMongoRepository<Bank> _bank;
        public BankService(IMongoRepository<Bank> bank)
        {
            _bank = bank;
        }
        public async Task<Bank> Create(Bank bank)
        {
            await _bank.InsertOneAsync(bank);
            return bank;
        }

        public async Task<List<Bank>> Get()
        {
            return await _bank.FindAll();
        }

        public async Task<Bank> Get(string id)
        {
            return await _bank.FindByIdAsync(id);
        }
    }
}
