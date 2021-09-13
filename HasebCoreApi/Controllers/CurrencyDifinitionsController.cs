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
    public class CurrencyDefinitionsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public CurrencyDefinitionsController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }

        /// <summary>
        ///  Get Specific Bank Account With Id
        /// </summary>
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
                return Ok(await _serviceWrapper.CurrencyDefinition.Get(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public object GetWithBranchId([FromQuery] string branchId, DataSourceLoadOptions dataSource)
        {
            if (string.IsNullOrWhiteSpace(branchId) || branchId.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            try
            {
                var data = _serviceWrapper.CurrencyDefinition.GetWithbranch(branchId);
                return DataSourceLoader.Load(data, dataSource);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("err_branch_notFound") });
            }
        }

        /// <summary>
        /// Insert Your New Bank Account
        /// </summary>
        /// <remarks>
        /// Send Token To Get UserId And Authorization
        /// 
        ///     {
        ///         BranchId:"24 Length",
        ///         CurrencyId:"24 Length",
        ///         IsBase:"bool" => Default true,
        ///         BaseRate: long => int,
        ///         IsActive:"bool" => Default True
        ///     }
        /// 
        /// 
        /// </remarks>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var currencyDefinition = new CurrencyDefinition();

            try
            {
                currencyDefinition.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, currencyDefinition);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }


            if (!TryValidateModel(currencyDefinition))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _currencyDefinition = await _serviceWrapper.CurrencyDefinition.Create(currencyDefinition);
                return CreatedAtAction("Get", new { id = _currencyDefinition.Id }, _currencyDefinition);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }

        /// <summary>
        /// Update Your Existing currency Definitions
        /// </summary>
        /// <remarks>
        /// Send Token For Authorization
        /// 
        /// Send Which Field Is Updated
        /// 
        ///     {
        ///     
        ///         IsBase:"bool" => Default true,
        ///         BaseRate: long => int,
        ///         
        ///     }
        /// 
        /// 
        /// </remarks>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromForm]string key, [FromForm] string values)
        {
            
            
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }

            var currencyDefinition = await _serviceWrapper.CurrencyDefinition.Get(key);
            if (currencyDefinition == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, currencyDefinition);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(currencyDefinition))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                currencyDefinition.UpdateDate = DateTime.Now;
                currencyDefinition.UpdaterId = User.GetUserId();
                var _currencyDefinition = await _serviceWrapper.CurrencyDefinition.Update(currencyDefinition);
                return Ok(_currencyDefinition);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] string key)
        {
            try
            {
                await _serviceWrapper.CurrencyDefinition.Delete(key);
                return Ok();
            }
            catch (IdLengthNotEqual)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("error_id_length_false") });
            }
        }
    }
}
