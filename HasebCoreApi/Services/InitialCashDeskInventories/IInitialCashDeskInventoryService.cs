using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.InitialCashDeskInventories
{
    public interface IInitialCashDeskInventoryService
    {
        Task<InitialCashDeskInventory> Get(string id);
        IQueryable<object> GetBranch(string BranchId);
        Task<InitialCashDeskInventory> Create(InitialCashDeskInventory initialInventoryCore);
        Task<InitialCashDeskInventory> Update(InitialCashDeskInventory initialInventoryCore);
        Task Delete(string id);
    }
}
