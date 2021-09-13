using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.InitialPosInventories
{
    public interface IInitialPosInventoryService
    {
        Task<InitialPosInventory> Get(string id);
        IQueryable<object> GetBranch(string BranchId);
        Task<InitialPosInventory> Create(InitialPosInventory initialPosInventory);
        Task<InitialPosInventory> Update(InitialPosInventory initialPosInventory);
        Task Delete(string id);
    }
}
