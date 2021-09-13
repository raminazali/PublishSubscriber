using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.InitialInventoryCores
{
    public class InitialInventoryCoreService : IInitialInventoryCoreService
    {
        readonly IMongoRepository<InitialPosInventory> _initialPosInventoryRepo;
        readonly IMongoRepository<InitialInventoryCore> _initialInventoryCoreRepo;
        readonly IMongoRepository<InitialBankInventory> _initialBankInventoryRepo;
        readonly IMongoRepository<InitialCashDeskInventory> _initialCashDeskInventoryRepo;
        readonly IMongoRepository<InitialPersonInventory> _initialPersonInventoryRepo;
        public InitialInventoryCoreService(
        IMongoRepository<InitialPosInventory> initialPosInventoryRepo,
         IMongoRepository<InitialInventoryCore> initialInventoryCoreRepo,
         IMongoRepository<InitialBankInventory> initialBankInventoryRepo,
         IMongoRepository<InitialCashDeskInventory> initialCashDeskInventoryRepo,
         IMongoRepository<InitialPersonInventory> initialPersonInventoryRepo
            )
        {
            _initialPosInventoryRepo = initialPosInventoryRepo;
            _initialInventoryCoreRepo = initialInventoryCoreRepo;
            _initialBankInventoryRepo = initialBankInventoryRepo;
            _initialCashDeskInventoryRepo = initialCashDeskInventoryRepo;
            _initialPersonInventoryRepo = initialPersonInventoryRepo;
        }
        public Task<InitialInventoryCore> Create(InitialInventoryCore initialInventoryCore)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<InitialInventoryCore> Get(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<object> GetBranch(string BranchId)
        {
            throw new NotImplementedException();
        }

        public Task<InitialInventoryCore> Update(InitialInventoryCore initialInventoryCore)
        {
            throw new NotImplementedException();
        }
    }
}
