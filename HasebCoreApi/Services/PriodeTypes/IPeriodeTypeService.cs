using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.PriodeTypes
{
    public interface IPeriodeTypeService
    {
        Task<PeriodType> Create(PeriodType periodeType);
        Task<PeriodType> Update(PeriodType periodeType);
        Task<List<PeriodType>> GetBranch(string branchId);
        Task<PeriodType> Get(string id);
        Task Delete(string id);
    }
}
