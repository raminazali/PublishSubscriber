using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.InitialInventoryCores
{
    public interface IInitialInventoryCoreService
    {
        Task<InitialInventoryCore> Get(string id);
        IQueryable<object> GetBranch(string BranchId);
        Task<InitialInventoryCore> Create(InitialInventoryCore initialInventoryCore);
        Task<InitialInventoryCore> Update(InitialInventoryCore initialInventoryCore);
        Task Delete(string id);
    }
}
