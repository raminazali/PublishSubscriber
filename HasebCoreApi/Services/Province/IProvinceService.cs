using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi
{
    public interface IProvinceService
    {
        Task<Province> Exists(string name);
        Task<List<Province>> Get();
        Task<Province> Get(int code);
    }

}