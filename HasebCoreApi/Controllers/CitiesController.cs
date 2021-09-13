using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HasebCoreApi.DTO.Common;
using HasebCoreApi.Helpers;
using HasebCoreApi.Localize;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;


namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    [Authorize]
    public class CitiesController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer _localizer;

        public CitiesController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }

        // GET: api/<CitiesController>
        /// <summary>
        /// Get all cities
        /// </summary>
        [HttpGet]
        public async Task<object> Get()
        {

            return await _serviceWrapper.City.Get();
        }

        /// <summary>
        /// Get a city by Id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            return Ok(await _serviceWrapper.City.Get(id));
        }

        /// <summary>
        /// Get cities by province
        /// </summary>

        [HttpGet("byProvince/{provinceCode}")]
        public async Task<IEnumerable<City>> GetByProvince(int provinceCode)
        {
            return await _serviceWrapper.City.GetByProvince(provinceCode);
        }
    }
}
