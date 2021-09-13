using System;
using System.Threading.Tasks;
using HasebCoreApi.DTO.Common;
using HasebCoreApi.Helpers;
using HasebCoreApi.Localize;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using HasebCoreApi.Services.CommodityGroups;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    [Authorize]
    public class CommodityGroupsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public CommodityGroupsController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }

        /// <summary>
        ///     Get All commodity Groups
        /// </summary>
        [HttpGet("GetAll")]
        public async Task<object> Get(DataSourceLoadOptions test)
        {
            try
            {
                var data = await _serviceWrapper.CommodityGroup.Get();
                return DataSourceLoader.Load(data , test);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        ///     Get commodity groups with parent id and branch id.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string parentId, [FromQuery] string branchId)
        {
            try
            {
                return Ok(await _serviceWrapper.CommodityGroup.GetByParent(parentId, branchId));
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        ///     Get commodity group with specific id.
        /// </summary>
        /// <remarks>
        /// Send Token Jwt For Getting User Id and Authorization
        /// 
        /// Sample request:
        ///
        ///     {   
        ///         Send CommodityGroupId  
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Get By id is successfull</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            try
            {
                return Ok(await _serviceWrapper.CommodityGroup.Get(id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        ///     Create a commodity group.
        /// </summary>
        /// <remarks>
        /// Send Token Jwt For Getting User Id and Authorization
        /// 
        /// Sample request:
        ///
        ///     {   
        ///         BranchId:"5fa9043dd06b9463ce9cfeba",
        ///         Level:0,
        ///         Code:"123",
        ///         Name:"لوازم بهداشتی",
        ///         SubToId:"5fae4d8d9c650b6cf41e972f" => can be null
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Create is successfull</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 4000, Message = "Data format is not valid." }
        ///     
        ///     { Code = 4001, Message = "Data validation error." } 
        ///     
        ///     { Code = 0, Message = "Commodity group is not found." } 
        ///     
        /// </response>
        /// 
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
             var commodityGroup = new CommodityGroup();
            try
            {
                commodityGroup.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, commodityGroup);
            }
            catch (Exception)
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(commodityGroup))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                await _serviceWrapper.CommodityGroup.Create(commodityGroup);
                return CreatedAtAction("Get", new { id = commodityGroup.Id }, commodityGroup);
            }
            catch (CommodityGroupNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_commodityGroup_notFound") });
            }
        }

        /// <summary>
        ///     Update commodity group with Specific Id.
        /// </summary>
        /// <remarks>
        /// Send Token Jwt For Authorization
        /// 
        /// Sample request:
        ///
        ///     {   
        ///     
        ///         Send Field That You want to Update It
        ///         
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Update is successfull</response>
        /// <response code="400">
        /// </response>
        /// 
        [HttpPut]
        public async Task<IActionResult> Put([FromForm]string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            var commodityGroup = await _serviceWrapper.CommodityGroup.Get(key);
            if (commodityGroup == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, commodityGroup);
            }
            catch (Exception)
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(commodityGroup))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                await _serviceWrapper.CommodityGroup.Update(commodityGroup);
                return Ok(commodityGroup);
            }
            catch (Exception)
            {
                throw;
            }
        }
        // DELETE api/<CitiesController>/5
        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] string key)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            try
            {
                await _serviceWrapper.CommodityGroup.Delete(key);
                return Ok();
            }
            catch (CommodityGroupReferencedToItselfException)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("err_commoditygroup_self_reference") });
            }
            catch (CommodityGroupReferencedToCommodityException ex)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("err_commoditygroup_commodity_reference"), Data = ex.Commodity.NameOne });
            }
        }
    }
}
