using Mongo.Models;
using Mongo.Pagination;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ResponseApi.Interface
{
    public interface IResponseApi
    {
        Task<(int, List<CustomerRequestBody>)> GetData(PaginationFilter filter);
    }
}
