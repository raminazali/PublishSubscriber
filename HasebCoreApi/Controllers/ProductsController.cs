using System.Threading.Tasks;
using HasebCoreApi.DTO.Common;
using HasebCoreApi.Helpers;
using HasebCoreApi.Localize;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource>  _localizer;
        public ProductsController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer;
            _serviceWrapper = serviceWrapper;
        }
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var data = await _serviceWrapper.Product.Get();
                return data;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            try
            {
                return Ok(await _serviceWrapper.Product.Get(id));

            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var product = new Product();
            try
            {
                JsonConvert.PopulateObject(values, product);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(product))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                await _serviceWrapper.Product.Create(product);
                return CreatedAtAction("Get", new { id = product.Id}, product);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromForm]string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            var product = await _serviceWrapper.Product.Get(key);
            if (product == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, product);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(product))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                await _serviceWrapper.Product.Update(product);
                return Ok(product);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        // DELETE api/<CitiesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok("Delete Not Completed");
        }
    }
}