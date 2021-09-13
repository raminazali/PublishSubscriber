using HasebCoreApi.Models;
using System.Threading.Tasks;
using HasebCoreApi.Helpers;
using System.Collections.Generic;
using MongoDB.Bson;
using System;
using System.Linq;

namespace HasebCoreApi
{
    public class CityService : ICityService
    {
        private readonly IMongoRepository<City> _cityRepo;
        private readonly IMongoRepository<Province> _provinceRepo;

        public CityService(IMongoRepository<City> cityRepo, IMongoRepository<Province> provinceRepo)
        {
            _cityRepo = cityRepo;
            _provinceRepo = provinceRepo;
        }

        public async Task<Object> Get()
        {
            var result = from a in _cityRepo.AsQueryable()
                         join b in _provinceRepo.AsQueryable()
                         on a.ProvinceCode equals b.Code
                         into joinedProvince
                         select new
                         {
                             Province = joinedProvince.First(),
                             a.Id,
                             a.Name
                         };

            return await result.ToListAsyncSafe();
        }

        public async Task<City> Get(string id)
        {
            return await _cityRepo.FindByIdAsync(id);
        }

        public async Task<IEnumerable<City>> GetByProvince(int provinceCode)
        {
            var data =  await _cityRepo.AsQueryable().Where(x => x.ProvinceCode == provinceCode).ToListAsyncSafe();
            return data;
        }

        public async Task<City> Exists(string name)
        {
            return await _cityRepo.FindOneAsync(x => x.Name == name);
        }
    }

    /// <summary>
    /// City is duplicated
    /// </summary>
    public class CityDuplicateException : Exception { public City City { get; set; } }
}






