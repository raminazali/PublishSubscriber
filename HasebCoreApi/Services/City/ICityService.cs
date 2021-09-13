using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi
{
    public interface ICityService
    {
        Task<City> Exists(string name);
        Task<object> Get();
        Task<City> Get(string id);
        Task<IEnumerable<City>> GetByProvince(int provinceCode);
    }

}