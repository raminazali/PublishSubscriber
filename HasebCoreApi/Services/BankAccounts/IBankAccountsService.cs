using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.BankAccounts
{
    public interface IBankAccountsService
    {
        /// <summary>
        ///  Update The bank Account 
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <exception cref="BranchNotFoundException">No branchId sent from front and its come null in model</exception>
        /// <returns>returns updated bank account informations</returns>
        Task<BankAccount> Update(BankAccount bankAccount);
        /// <summary>
        /// Create new Bank Account 
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <exception cref="BranchNotFoundException">No branchId sent from front and its come null in model</exception>
        /// <returns>returns Created bank Account Informations</returns>
        Task<BankAccount> Create(BankAccount bankAccount);
        /// <summary>
        /// Create new Bank Account 
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <exception cref="BranchNotFoundException">No branch Found In branch table</exception>
        /// <returns>returns Created bank Account Informations</returns>
        IQueryable<object> GetBranch(string branchId);
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BankAccount> Get(string id);
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(string id);
    }
}
