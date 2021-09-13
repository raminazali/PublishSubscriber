using HasebCoreApi.Models;
using System.Threading.Tasks;
using HasebCoreApi.Helpers;
using System;
using System.Linq;
using MongoDB.Driver;

namespace HasebCoreApi
{
    public class BranchService : IBranchService
    {
        private readonly IMongoRepository<Branch> _branchRepo;
        private readonly IMongoRepository<UserInfo> _userRepo;
        private readonly IMongoRepository<Plan> _plan;

        public BranchService(IMongoRepository<Branch> branchRepo, IMongoRepository<UserInfo> userRepo, IMongoRepository<Plan> plan)
        {
            _branchRepo = branchRepo;
            _userRepo = userRepo;
            _plan = plan;
        }

        public async Task<object> GetDashboard(string userId)
        {
            //.Where(x => x.UserId.Any(x => x.UserId == userId))                            *******************FixMe*******************
            var result = from a in _branchRepo.AsQueryable()
                         join b in _userRepo.AsQueryable()
                         on a.OwnerId equals b.Id
                         join c in _userRepo.AsQueryable()
                         on a.BuyerId equals c.Id
                         where a.OwnerId == userId || a.BuyerId == userId
                         select new
                         {
                             a.Id,
                             a.Name,
                             a.StartDate,
                             Owner = new { b.Id, Name = b.FirstName + " " + b.LastName, Username = b.UserName },
                             Buyer = new { c.Id, Name = c.FirstName + " " + c.LastName, Username = c.UserName },
                             Username = b.UserName,
                             Users = a.UserId
                         };

            var q = await result.ToListAsyncSafe();
            return q.Select(x => new
            {
                x.Id,
                x.Buyer,
                x.Name,
                x.Owner,
                x.StartDate
                //Me = x.Users.FirstOrDefault(x => x.UserId == userId)                     *******************FixMe*******************
            });
        }

        public async Task<Branch> Get(string id)
        {
            return await _branchRepo.FindByIdAsync(id);
        }

        public async Task<Branch> Exists(string name)
        {
            return await _branchRepo.FindOneAsync(x => x.Name == name);
        }

        public async Task<Branch> Create(Branch branch)
        {
            await _branchRepo.InsertOneAsync(branch);
            return branch;
        }

        public async Task<Branch> Update(Branch branch)
        {
            var _name = branch.Name.Trim();
            var dup = await _branchRepo.FindOneAsync(x => x.Name == _name && x.Id != branch.Id);
            if (dup != null)
            {
                throw new BranchDuplicateException { Branch = dup };
            }

            await _branchRepo.ReplaceOneAsync(branch);
            return branch;
        }

        public async Task<object> Getphone(string phonenumber)
        {
            var data = await _userRepo.FindOneAsync(x => x.Mobile == phonenumber);
            if (data == null)
            {
                throw new NoInformationNumberException();
            }
            return new { data.FirstName, data.LastName, data.UserName, data.Id, data.Mobile };
        }
    }

    /// <summary>
    /// Branch is duplicated
    /// </summary>
    public class BranchDuplicateException : Exception { public Branch Branch { get; set; } }
    public class NoInformationNumberException : Exception { }
}






