using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.InitialCashDeskInventories
{
    public class InitialCashDeskInventoryService : IInitialCashDeskInventoryService
    {
        private readonly IMongoRepository<InitialCashDeskInventory> _initialCashDeskInventory;
        public InitialCashDeskInventoryService(IMongoRepository<InitialCashDeskInventory> initialCashDeskInventory)
        {
            _initialCashDeskInventory = initialCashDeskInventory;       
        }
        public Task<InitialCashDeskInventory> Create(InitialCashDeskInventory initialInventoryCore)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id)
        {
            await _initialCashDeskInventory.DeleteByIdAsync(id);
        }

        public Task<InitialCashDeskInventory> Get(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<object> GetBranch(string BranchId)
        {
            throw new NotImplementedException();
        }

        public Task<InitialCashDeskInventory> Update(InitialCashDeskInventory initialInventoryCore)
        {
            throw new NotImplementedException();
        }
    }
}
