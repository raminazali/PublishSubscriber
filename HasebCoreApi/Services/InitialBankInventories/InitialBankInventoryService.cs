using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.InitialBankInventories
{
    public class InitialBankInventoryService : IInitialBankInventoryService
    {
        private readonly IMongoRepository<InitialBankInventory> _initialBankInventory;
        public InitialBankInventoryService(IMongoRepository<InitialBankInventory> initialBankInventory)
        {
            _initialBankInventory = initialBankInventory;
        }
        public Task<InitialBankInventory> Create(InitialBankInventory initialInventoryCore)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id)
        {
            await _initialBankInventory.DeleteByIdAsync(id);
        }

        public Task<InitialBankInventory> Get(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<object> GetBranch(string BranchId)
        {
            throw new NotImplementedException();
        }

        public Task<InitialBankInventory> Update(InitialBankInventory initialInventoryCore)
        {
            throw new NotImplementedException();
        }
    }
}
