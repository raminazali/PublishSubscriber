using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi
{
    public interface ITitleService
    {
        Task<Title> Create(Title title);
        Task<Title> Get(string id);
        Task<List<Title>> Get();
        Task<Title> Update(Title title);
    }

}