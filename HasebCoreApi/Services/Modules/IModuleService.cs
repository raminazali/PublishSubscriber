using System.Collections.Generic;
// using System.Reflection;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.Modules
{
    public interface IModuleService
    {
        Task<List<Module>> Get();
        Task<Module> Get(string id);
        Task Create(Module module);
        Task Update(Module module);
    }
}