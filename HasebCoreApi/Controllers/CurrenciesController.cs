using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
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
    public class CurrenciesController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public CurrenciesController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }
        /// <summary>
        ///     Get All Currency List
        /// </summary>
        /// <remarks>
        /// 
        /// 
        /// OutPut Example :
        /// 
        ///     {
        ///         "NameOne": "لک البانی",
        ///         "NameTwo": "لک البانی",
        ///         "Abbreviation": "ALL",
        ///         "ForeignExchangeCode": 8,
        ///         "Id": "5fb8adf210b85712bcfd2f06"
        ///     }
        /// 
        /// 
        ///     {
        ///         Send Jwt Token For Auhtorization
        ///     }
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions dataSource)
        {
            try
            {
                return Ok(await _serviceWrapper.Currency.Get());
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        ///     Get Currency By Id
        /// </summary>
        /// <remarks>
        ///     
        ///     {
        ///         Send Jwt Token For Authorization
        ///     }
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }

            try
            {
                return Ok(await _serviceWrapper.Currency.Get(id));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
