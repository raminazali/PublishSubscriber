using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.Poses
{
    public interface IPosService
    {
        //Task<List<object>> Get();
        Task<Pos> Get(string id);
        IQueryable<object> GetBranch(string branchId);
        Task<Pos> Create(Pos pos);
        Task<Pos> Update(Pos pos);
        Task Delete(string id);
    }
}
