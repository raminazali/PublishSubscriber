using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HasebCoreApi.Localize;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    public class InitialInventoryCoresController : ControllerBase
    {
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly IServiceWrapper _serviceWrapper;
        public InitialInventoryCoresController(IStringLocalizer<Resource> localizer,IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        { 
        }
    }
}
