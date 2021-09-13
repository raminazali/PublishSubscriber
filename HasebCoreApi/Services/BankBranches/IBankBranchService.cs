using HasebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Services.BankBranches
{
    public interface IBankBranchService
    {
        /// <summary>
        ///  Delete Bank Branch if Have No Reference
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="BankBranchIdIsReferencedException">Bank branch in Referenced to Bank Account</exception>
        /// <returns>Nothing</returns>
        Task Delete(string id);
        /// <summary>
        /// Get Bank Branch by specific Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BankBranch> Get(string id);
        /// <summary>
        ///  Get List of Bank Branch with Branch Id and Bank Name Filter
        /// </summary>
        /// <param name="BranchId"></param>
        /// <param name="BankName"></param>
        /// <exception cref="BranchNotFoundException">No Branch Found for This Branch Id</exception>
        /// <returns>returns Arrays of BankBranch List</returns>
        IEnumerable<object> GetByBranch(string BranchId, string BankName);
        /// <summary>
        /// Create New Bank Branch
        /// </summary>
        /// <param name="bankBranch"></param>
        /// <returns>returns list of bankbranch that in created</returns>
        Task<BankBranch> Create(BankBranch bankBranch);
        /// <summary>
        /// Update Bank Branch 
        /// </summary>
        /// <param name="bankBranch">fields that updated</param>
        /// <returns>returns updated bank branch</returns>
        Task<BankBranch> Update(BankBranch bankBranch);
    }
}
