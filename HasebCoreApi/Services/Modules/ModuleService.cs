using System.Collections.Generic;
// using System.Reflection;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.Modules
{
    public class ModuleService : IModuleService
    {
        private readonly IMongoRepository<Module> _moduleRepo;
        public ModuleService(IMongoRepository<Module> moduleRepo)
        {
            _moduleRepo = moduleRepo;
        }

        public async Task Create(Module module)
        {
            await _moduleRepo.InsertOneAsync(module);
        }

        public async Task<List<Module>> Get()
        {
            return await _moduleRepo.FindAll();
        }

        public async Task<Module> Get(string id)
        {
            return await _moduleRepo.FindByIdAsync(id);
        }

        public async Task Update(Module module)
        {
            await _moduleRepo.ReplaceOneAsync(module);
        }
    }
}