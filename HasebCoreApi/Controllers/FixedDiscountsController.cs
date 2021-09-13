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
    public class FixedDiscountsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer _localizer;
        public FixedDiscountsController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }
        /// <summary>
        ///  Get The All Fixed Discounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Get([FromQuery] string branchId, DataSourceLoadOptions dataSourceLoad)
        {
            try
            {
                var data = await _serviceWrapper.FixedDiscount.GetBranch(branchId);
                return DataSourceLoader.Load(data, dataSourceLoad);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        /// <summary>
        ///  
        ///     Get Specific Discount
        /// 
        /// </summary>
        /// <remarks>
        /// 
        ///     {
        ///         Send Id To Get Specific Discount
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
                return Ok(await _serviceWrapper.FixedDiscount.Get(id));
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        ///  Insert New Fixed Discount
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For Authorization And UserId
        /// 
        /// 
        ///     {
        ///         
        ///         BranchId:"24length",
        ///         Code:"varchar(max)",
        ///         NameOne:"Test Name",
        ///         NameTwo?:"its Optional",
        ///         percent?:"int And Optional",
        ///         Amount?:"Optional",
        ///         IsActive?:"Default True",
        ///         UpdaterId?:"24legth",
        ///         
        ///     }
        /// 
        /// </remarks>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var fixedDiscount = new FixedDiscount();

            try
            {
                fixedDiscount.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, fixedDiscount);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(fixedDiscount))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _bankAccount = await _serviceWrapper.FixedDiscount.Create(fixedDiscount);
                return CreatedAtAction("Get", new { id = _bankAccount.Id }, _bankAccount);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        /// <summary>
        ///  Update The Fixed Discount
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For Authorization
        /// 
        /// Send Which field Need To update
        /// 
        ///     {
        ///         
        ///         BranchId:"24length",
        ///         Code:"varchar(max)",
        ///         NameOne:"Test Name",
        ///         NameTwo?:"its Optional",
        ///         
        ///     }
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
            var fixedDiscount = await _serviceWrapper.FixedDiscount.Get(key);
            if (fixedDiscount == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }
            fixedDiscount.UpdaterId = User.GetUserId();
            fixedDiscount.UpdateDate = DateTime.Now;
            try
            {
                JsonConvert.PopulateObject(values, fixedDiscount);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(fixedDiscount))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                fixedDiscount.UpdateDate = DateTime.Now;
                fixedDiscount.UpdaterId = User.GetUserId();
                var _fixedDiscount = await _serviceWrapper.FixedDiscount.Update(fixedDiscount);
                return Ok(_fixedDiscount);
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
                await _serviceWrapper.FixedDiscount.Delete(key);
                return Ok();
            }
            catch (IdLengthNotEqual)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("error_id_length_false") });
            }
        }
    }
}
