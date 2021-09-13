using HasebCoreApi.Helpers;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.RealPersons
{
    public class LegalRealPersonService : ILegalRealPersonService
    {
        private readonly IMongoRepository<LegalRealPerson> _realPerson;
        private readonly IMongoRepository<Branch> _branch;
        private readonly IMongoRepository<InitialPersonInventory> _initialPersonInventory;
        public LegalRealPersonService(IMongoRepository<LegalRealPerson> realPerson, IMongoRepository<Branch> branch, IMongoRepository<InitialPersonInventory> initialPersonInventory)
        {
            _realPerson = realPerson;
            _branch = branch;
            _initialPersonInventory = initialPersonInventory;
        }
        public async Task Create(LegalRealPerson realPerson)
        {
            realPerson.Code = "p" + realPerson.Code;
            Array.Sort(realPerson.DetailedGroup);
            await _realPerson.InsertOneAsync(realPerson);
        }

        public async Task<List<LegalRealPerson>> GetBranch(string branchId, bool isLegal)
        {
            var branch = await _branch.FindByIdAsync(branchId);
            if (branch == null) throw new BranchNotFoundException();
            return await _realPerson.AsQueryable().Where(x => x.BranchId == branchId && x.IsLegal.Equals(isLegal)).ToListAsyncSafe();
        }

        public async Task<LegalRealPerson> Get(string id)
        {
            var realperson = await _realPerson.FindByIdAsync(id);
            if (realperson == null) throw new NoPersonFoundException();
            return realperson;
        }

        public async Task Update(LegalRealPerson realPerson)
        {
            realPerson.Code = "p" + realPerson.Code;
            await _realPerson.ReplaceOneAsync(realPerson);
        }

        public async Task<List<RelatedPerson>> GetRelatedPerson(string userid)
        {
            return await _realPerson.AsQueryable().Where(x => x.Id == userid).SelectMany(x => x.RelatedPerson).ToListAsyncSafe();
        }

        public async Task Delete(string id)
        {
            string[] keys = id.Split(",");
            foreach (var item in keys)
            {
                if (string.IsNullOrWhiteSpace(item) || item.Length != 24) throw new IdLengthNotEqual();

                var value = await _initialPersonInventory.FindByIdAsync(item);

                if (value != null) throw new PersonReferencedToinitialPersonException();

                await _realPerson.DeleteByIdAsync(item);
            }
        }
    }
}
public class NoPersonFoundException : Exception { }
public class PersonReferencedToinitialPersonException : Exception { }