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
    public class BankBranchsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public BankBranchsController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }

        /// <summary>
        /// Get Bank Branch Names with this api Just Pass The BranchId
        /// </summary>
        /// <param name="BranchId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromQuery] string BranchId, [FromQuery] string BankName)
        {
            try
            {
                return Ok(_serviceWrapper.BankBranch.GetByBranch(BranchId, BankName));
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }

        /// <summary>
        /// Insert New Bank Branch with this api
        /// </summary>
        /// <remarks>
        /// Send Jwt Token
        /// 
        ///     {
        ///         BranchId:"24 length",
        ///         BankbranchName:"varchar(max)",
        ///         BankBranchCode:"varchar(7)",
        ///         BankName:"varchar(max)"
        ///     }
        /// 
        /// </remarks>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var bankBranch = new BankBranch();
            try
            {
                JsonConvert.PopulateObject(values, bankBranch);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            //bank.UserId = User.GetUserId();

            if (!TryValidateModel(bankBranch))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _bankBranch = await _serviceWrapper.BankBranch.Create(bankBranch);
                return CreatedAtAction("Get", _bankBranch);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        [HttpPut]
        public async Task<IActionResult> Post([FromForm] string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            var bankBranch = await _serviceWrapper.BankBranch.Get(key);
            if (bankBranch == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, bankBranch);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(bankBranch))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                await _serviceWrapper.BankBranch.Update(bankBranch);
                return Ok(bankBranch);
            }
            catch (BranchDuplicateException ex)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_general_duplicate"), Data = ex.Branch });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] string key)
        {
            try
            {
                await _serviceWrapper.BankBranch.Delete(key);
                return Ok();
            }
            catch (BankBranchIdIsReferencedException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_bank_id_referenced") });
            }
        }
    }
}
