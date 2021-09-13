using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HasebCoreApi.Helpers;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.Units
{
    public class UnitService : IUnitService
    {
        private readonly IMongoRepository<Unit> _unitRepo;
        private readonly IMongoRepository<Commodity> _commodityRepo;
        private readonly IMongoRepository<Branch> _branch;
        public UnitService(IMongoRepository<Unit> unitRepo, IMongoRepository<Branch> branch, IMongoRepository<Commodity> commodityRepo)
        {
            _unitRepo = unitRepo;
            _branch = branch;
            _commodityRepo = commodityRepo;
        }
        public async Task Create(Unit unit)
        {
            var branch = _branch.FindById(unit.BranchId);
            if (branch == null)
            {
                throw new BranchNotFoundException();
            }
            await _unitRepo.InsertOneAsync(unit);
        }

        public async Task Delete(string id)
        {
            var keys = id.Split(",");
            foreach (var item in keys)
            {
                if (string.IsNullOrWhiteSpace(item) || item.Length != 24) throw new IdLengthNotEqual();

                var comm = await _commodityRepo.FindOneAsync(x => x.UnitMainId == id || x.UnitSubId == id);

                if (comm != null)
                {
                    throw new UnitIdIsReferencedException();
                }

                await _unitRepo.DeleteByIdAsync(item);
            }

        }

        public async Task<List<Unit>> GetBranch(string branchId)
        {
            var branch = await _branch.FindByIdAsync(branchId);
            if (branch == null) throw new BranchNotFoundException();

            return await _unitRepo.AsQueryable().Where(x => x.BranchId == branchId).ToListAsyncSafe();
        }

        public async Task<Unit> Get(string id)
        {
            var unit = await _unitRepo.FindByIdAsync(id);
            if (unit == null) throw new NoSuchUnitFoundException();
            return unit;
        }

        public async Task Update(Unit unit)
        {
            await _unitRepo.ReplaceOneAsync(unit);
        }
    }
}
public class NoSuchUnitFoundException : Exception { }
public class UnitIdIsReferencedException : Exception { }
public class IdLengthNotEqual : Exception { }

