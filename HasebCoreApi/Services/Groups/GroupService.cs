using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.Groups
{
    public class GroupService: IGroupService
    {
        private readonly IMongoRepository<Group> _group;
        public GroupService(IMongoRepository<Group> group)
        {
            _group = group;
        }

        public async Task Create(Group group)
        {
            await _group.InsertOneAsync(group);
        }

        public async Task<List<Group>> Get()
        {
            return await _group.FindAll();
        }

        public async Task<Group> Get(string id)
        {
            return await _group.FindByIdAsync(id);
        }

        public async Task Update(Group group)
        {
            await _group.ReplaceOneAsync(group);
        }
    }
}