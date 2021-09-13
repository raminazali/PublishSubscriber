using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.InitialBankInventories
{
    public interface IInitialBankInventoryService
    {
        Task<InitialBankInventory> Get(string id);
        IQueryable<object> GetBranch(string BranchId);
        Task<InitialBankInventory> Create(InitialBankInventory initialInventoryCore);
        Task<InitialBankInventory> Update(InitialBankInventory initialInventoryCore);
        Task Delete(string id);
    }
}
