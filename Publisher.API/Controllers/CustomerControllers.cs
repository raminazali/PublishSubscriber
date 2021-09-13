using Microsoft.AspNetCore.Mvc;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Publisher.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerControllers : ControllerBase
    {
        private readonly IMongoRepository<CustomerRequestBody> _collection;
        private readonly IResponseApi responseApi;

        public CustomerControllers(IResponseApi responseApi)
        {
            this.responseApi = responseApi;
        }
       
        
        [HttpPost]
        public async Task Post()
        {
            Ok();
        }

        [HttpGet]
        public async Task<object> Get([FromQuery] PaginationFilter filter)
        {
            try
            {
                var ListOfData = await responseApi.GetData(filter);
                return new {data = ListOfData.Item2 , count = ListOfData.Item1 , pageSize = filter.PageSize, pageNumber=filter.PageNumber } ;
            }
            catch (Exception ex){

                return " Something Went Wrong "+ ex.Message;
            }
            
        }
    }
}
