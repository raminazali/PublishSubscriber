using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.Products
{
    public interface IProductService
    {
        Task<List<Product>> Get();
        Task<Product> Get(string id);
        Task Create(Product product);
        Task Update(Product product);
    }
}