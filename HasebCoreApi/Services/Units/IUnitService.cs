using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.Units
{
    public interface IUnitService
    {
        /// <summary>
        /// Create New unit for special Branch
        /// </summary>
        /// <param name="unit"></param>
        /// <exception cref="BranchNotFoundException">Branch Id Not Found Or Not Exists</exception>
        /// <returns>returns list of units after creation</returns>
        Task Create(Unit unit);
        /// <summary>
        /// Update unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>returns list of units after Updating</returns>
        Task Update(Unit unit);
        /// <summary>
        /// Get All units For  Specific Branch id
        /// </summary>
        /// <param name="branchId"></param>
        /// <exception cref="BranchNotFoundException">Branch Id Not Found Or Not Exists</exception>
        /// <returns>returns list of units with BranchId Filter</returns>
        Task<List<Unit>> GetBranch(string branchId);
        /// <summary>
        /// Update unit
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NoSuchUnitFoundException">nothing Found for Selected unit</exception>
        /// <returns>returns unit</returns>
        Task<Unit> Get(string id);
        /// <summary>
        /// Update unit
        /// </summary>
        /// <param name="keys"></param>
        /// <exception cref="IdLengthNotEqual">If Id Length greater or less than 24</exception>
        /// <exception cref="UnitIdIsReferencedException">When the specific unit used in another Tables</exception>
        /// <returns>nothing</returns>
        Task Delete(string keys);
    }
}