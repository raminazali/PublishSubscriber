using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IMongoRepository<Product> _product;
        public ProductService(IMongoRepository<Product> product)
        {
            _product = product;
        }

        public async Task Create(Product product)
        {
            await _product.InsertOneAsync(product);
        }

        public async Task<List<Product>> Get()
        {
            return await _product.FindAll();
        }

        public async Task<Product> Get(string id)
        {
            return await _product.FindByIdAsync(id);
        }

        public async Task Update(Product product)
        {
            await _product.ReplaceOneAsync(product);
        }
    }
}