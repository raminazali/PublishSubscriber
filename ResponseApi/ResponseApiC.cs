using Microsoft.Extensions.DependencyInjection;
using Mongo;
using Mongo.Models;
using Mongo.Pagination;
using MongoDB.Bson;
using ResponseApi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResponseApi
{
    public class ResponseApiC : IResponseApi
    {
        private readonly IMongoRepository<ReqDataMongo> _collections;
        public ResponseApiC(IServiceProvider provider)
        {
            var ResponseApiScope = provider.CreateScope();
            _collections = ResponseApiScope.ServiceProvider.GetRequiredService<IMongoRepository<ReqDataMongo>>();
        }

        public async Task<(int,List<ReqDataMongo>)> GetData(PaginationFilter filter)
        {
            var data = _collections.AsQueryable().Skip((filter.PageNumber - 1) * filter.PageSize)
               .Take(filter.PageSize).ToList();

            var totalRecords = _collections.AsQueryable().Count();

            return (totalRecords,data);


        }
    }
}
