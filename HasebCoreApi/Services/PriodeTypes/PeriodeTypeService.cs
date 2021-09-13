using HasebCoreApi.Helpers;
using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.PriodeTypes
{
    public class PeriodeTypeService : IPeriodeTypeService
    {
        private readonly IMongoRepository<PeriodType> _periodeType;
        private readonly IMongoRepository<Branch> _branch;
        public PeriodeTypeService(IMongoRepository<PeriodType> periodeType, IMongoRepository<Branch> branch)
        {
            _periodeType = periodeType;
            _branch = branch;
        }
        public async Task<PeriodType> Create(PeriodType periodeType)
        {
            var Name = await _periodeType.FindOneAsync(x => x.Name == periodeType.Name && x.BranchId == periodeType.BranchId);
            if (Name != null) throw new DuplicateNameAddedException();

            await _periodeType.InsertOneAsync(periodeType);
            return periodeType;
        }

        public async Task Delete(string id)
        {
            await _periodeType.DeleteByIdAsync(id);
        }

        public async Task<List<PeriodType>> GetBranch(string branchId)
        {
            var branch = await _branch.FindByIdAsync(branchId);
            if (branch == null) throw new BranchNotFoundException();

            return await _periodeType.AsQueryable().Where(x=>x.BranchId == branchId || x.BranchId == null).ToListAsyncSafe();
        }

        public async Task<PeriodType> Get(string id)
        {
            return await _periodeType.FindByIdAsync(id);
        }

        public async Task<PeriodType> Update(PeriodType periodeType)
        {
            await _periodeType.ReplaceOneAsync(periodeType);
            return periodeType;
        }
    }
}
public class DuplicateNameAddedException : Exception { }