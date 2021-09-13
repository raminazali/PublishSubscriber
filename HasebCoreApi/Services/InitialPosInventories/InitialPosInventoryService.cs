using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.InitialPosInventories
{
    public class InitialPosInventoryService : IInitialPosInventoryService
    {
        private readonly IMongoRepository<InitialPosInventory> _InitialPosRepo;
        public InitialPosInventoryService(IMongoRepository<InitialPosInventory> InitialPosRepo)
        {
            _InitialPosRepo = InitialPosRepo;
        }
        public Task<InitialPosInventory> Create(InitialPosInventory initialPosInventory)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id)
        {
            await _InitialPosRepo.DeleteByIdAsync(id);
        }

        public Task<InitialPosInventory> Get(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<object> GetBranch(string BranchId)
        {
            throw new NotImplementedException();
        }

        public Task<InitialPosInventory> Update(InitialPosInventory initialPosInventory)
        {
            throw new NotImplementedException();
        }
    }
}
