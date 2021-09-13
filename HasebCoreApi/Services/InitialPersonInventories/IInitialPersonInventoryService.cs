using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.InitialPersonInventories
{
    public interface IInitialPersonInventoryService
    {
        Task<InitialCashDeskInventory> Get(string id);
        IQueryable<object> GetBranch(string BranchId);
        Task<InitialCashDeskInventory> Create(InitialCashDeskInventory initialInventoryCore);
        Task<InitialCashDeskInventory> Update(InitialCashDeskInventory initialInventoryCore);
        Task Delete(string id);
    }
}
