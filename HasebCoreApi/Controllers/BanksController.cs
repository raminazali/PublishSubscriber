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
    public class BanksController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public BanksController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }
        /// <summary>
        ///  Get All Banks List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _serviceWrapper.Bank.Get());
        }
        /// <summary>
        ///  Get By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _serviceWrapper.Bank.Get(id));
        }
        /// <summary>
        /// Insert New Bank Name
        /// </summary>
        /// <remarks>
        /// 
        ///     {
        ///         Name:"varchar(max)"
        ///     }
        ///     
        /// </remarks>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var bank = new Bank();
            try
            {
                JsonConvert.PopulateObject(values, bank);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            //bank.UserId = User.GetUserId();

            if (!TryValidateModel(bank))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _bank = await _serviceWrapper.Bank.Create(bank);
                return CreatedAtAction("Get", new { id = _bank.Id }, _bank);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
    }
}
