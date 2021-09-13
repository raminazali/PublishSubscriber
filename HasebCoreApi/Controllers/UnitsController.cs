using System.Threading.Tasks;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HasebCoreApi.Helpers;
using Newtonsoft.Json;
using HasebCoreApi.DTO.Common;
using Microsoft.Extensions.Localization;
using HasebCoreApi.Localize;
using System;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using System.Collections.Generic;

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    [Authorize]
    public class UnitsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public UnitsController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<object> Get([FromQuery] string branchId, DataSourceLoadOptions test)
        {
            try
            {
                var data = await _serviceWrapper.Unit.GetBranch(branchId);
                return DataSourceLoader.Load(data, test);
                //return Ok(await _serviceWrapper.Unit.Get());
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        /// <summary>
        ///  Get Unit With Specific Id
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For Authorization
        /// 
        /// Sample request:
        ///
        ///    {  
        ///    
        ///        string id: "5fa9043dd06b9463ce9cfeba"
        ///        
        ///    }
        ///
        /// </remarks>
        /// <response code="200">Success To Get Unit</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            try
            {
                return Ok(await _serviceWrapper.Unit.Get(id));
            }
            catch (NoSuchUnitFoundException)
            {
                return NotFound(new GenericMessage { Code = 4005, Message = _localizer.GetString("error_no_unit_exists") });
            }
        }
        /// <summary>
        ///  Insert New Unit As You Need With This Api
        /// </summary>
        /// <remarks>
        /// UserId => Send Jwt Token we will Get UserId From Token
        /// 
        /// Sample request:
        ///
        ///      
        ///    
        ///        {
        ///             BranchId: "5fa9043dd06b9463ce9cfeba",
        ///             NameOne: "KG",
        ///             NameTwo: "Kilo Geram",
        ///             UnitCode: "123"
        ///        }    
        ///        
        ///    
        ///
        /// </remarks>
        /// <response code="200">Success To Insert New Unit</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var unit = new Unit();
            try
            {
                unit.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, unit);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(unit))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                await _serviceWrapper.Unit.Create(unit);
                return CreatedAtAction("Get", new { id = unit.Id }, unit);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        /// <summary>
        ///  Update Unit And Sent which fild That have Changed
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For Authorization
        /// 
        /// Sample request: 
        /// 
        ///         {
        ///             NameOne:"Its Example Name" 
        ///         }
        ///         
        /// </remarks>
        /// <response code="200">Success To Update Unit</response>
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            var unit = await _serviceWrapper.Unit.Get(key);
            if (unit == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, unit);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(unit))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                await _serviceWrapper.Unit.Update(unit);
                return Ok();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        // DELETE api/<CitiesController>/5
        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] string key)
        {
            try
            {
                await _serviceWrapper.Unit.Delete(key);
                return Ok();
            }
            catch (IdLengthNotEqual)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            catch (UnitIdIsReferencedException)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("err_unit_id_referenced") });
            }
        }
    }
}