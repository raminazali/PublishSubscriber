using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HasebCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InitialPersonInventoriesController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        public InitialPersonInventoriesController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }
        // GET: api/<InitialPersonInventoriesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<InitialPersonInventoriesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InitialPersonInventoriesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<InitialPersonInventoriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InitialPersonInventoriesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
