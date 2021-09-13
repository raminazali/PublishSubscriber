using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.RealPersons
{
    public interface ILegalRealPersonService
    {
        Task Create(LegalRealPerson realPerson);
        Task Update(LegalRealPerson realPerson);
        Task<List<LegalRealPerson>> GetBranch(string branchId, bool isLegal);
        Task<List<RelatedPerson>> GetRelatedPerson(string userid);
        Task<LegalRealPerson> Get(string id);
        Task Delete(string id);

    }
}
