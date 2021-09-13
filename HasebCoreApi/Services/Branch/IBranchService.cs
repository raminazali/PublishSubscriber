using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.DTO.Branch;
using HasebCoreApi.Models;

namespace HasebCoreApi
{
    public interface IBranchService
    {
        Task<Branch> Create(Branch branch);
        Task<Branch> Get(string id);
        Task<object> Getphone(string phonenumber);
        Task<object> GetDashboard(string UserId);
        Task<Branch> Update(Branch branch);
    }

}