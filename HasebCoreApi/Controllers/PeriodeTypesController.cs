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
    public class PeriodeTypesController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public PeriodeTypesController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }

        /// <summary>
        /// Send Request And Get All Periode Types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBranch([FromQuery] string BranchId)
        {
            try
            {
                return Ok(await _serviceWrapper.PeriodeType.GetBranch(BranchId));
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        /// <summary>
        /// Get Specific Periode With Id
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For Authorization
        /// 
        /// 
        ///     {                
        ///         Send Id And Get Specific Periode
        ///     }
        /// 
        /// </remarks>
        /// <returns>Something</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }

            try
            {
                return Ok(await _serviceWrapper.PeriodeType.Get(id));
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Insert New Periode Type
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For Authrization and Mine UserId
        /// 
        /// 
        ///     {     
        ///         BranchId?:"5fa9043dd06b9463ce9cfeba",
        ///         Name:"VarChar(70)"
        ///     }
        /// 
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var periodeType = new PeriodType();

            try
            {
                periodeType.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, periodeType);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            periodeType.UserId = User.GetUserId();

            if (!TryValidateModel(periodeType))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _periodType = await _serviceWrapper.PeriodeType.Create(periodeType);
                return CreatedAtAction("Get", new { id = _periodType.Id }, _periodType);
            }
            catch (DuplicateNameAddedException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_duplicate_name") });
            }
        }
        /// <summary>
        /// Update Periode Type With Id From This Api
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For Authrization 
        /// 
        /// Send What You want to Update
        /// 
        ///     {  
        ///         Name:"VarChar(70)"
        ///     }
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromForm]string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }

            var periodeType = await _serviceWrapper.PeriodeType.Get(key);
            if (periodeType == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, periodeType);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(periodeType))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _periodeType = await _serviceWrapper.PeriodeType.Update(periodeType);
                return Ok(_periodeType);
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
                await _serviceWrapper.PeriodeType.Delete(key);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
