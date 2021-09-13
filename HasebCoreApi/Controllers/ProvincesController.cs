using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HasebCoreApi.DTO.Common;
using HasebCoreApi.Helpers;
using HasebCoreApi.Localize;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    public class ProvincesController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer _localizer;

        public ProvincesController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }

        // GET: api/<ProvincesController>
        [HttpGet]
        public async Task<IEnumerable<Province>> Get()
        {
            return await _serviceWrapper.Province.Get();
        }

        // GET api/<ProvincesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int code)
        {
            return Ok(await _serviceWrapper.Province.Get(code));
        }
    }
}
