using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.InitialPersonInventories
{
    public class InitialPersonInventoryService : IInitialPersonInventoryService
    {
        private readonly IMongoRepository<InitialPersonInventory> _initialPersonInventory;
        public InitialPersonInventoryService(IMongoRepository<InitialPersonInventory> initialPersonInventory)
        {
            _initialPersonInventory = initialPersonInventory;
        }
        public Task<InitialCashDeskInventory> Create(InitialCashDeskInventory initialInventoryCore)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id)
        {
           await _initialPersonInventory.DeleteByIdAsync(id);
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
