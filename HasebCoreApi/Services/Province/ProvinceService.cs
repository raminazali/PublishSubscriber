using HasebCoreApi.Models;
using System.Threading.Tasks;
using HasebCoreApi.Helpers;
using System.Collections.Generic;
using MongoDB.Bson;
using System;

namespace HasebCoreApi
{
    public class ProvinceService : IProvinceService
    {
        private readonly IMongoRepository<Province> _provinceRepo;

        public ProvinceService(IMongoRepository<Province> provinceRepo)
        {
            _provinceRepo = provinceRepo;
        }

        public async Task<List<Province>> Get()
        {
            return await _provinceRepo.FindAll();
        }

        public async Task<Province> Get(int code)
        {
            return await _provinceRepo.FindOneAsync(x=>x.Code == code);
        }

        public async Task<Province> Exists(string name)
        {
            return await _provinceRepo.FindOneAsync(x => x.Name == name);
        }
    }

    /// <summary>
    /// Province is duplicated
    /// </summary>
    public class ProvinceDuplicateException : Exception { public Province Province { get; set; } }
}






