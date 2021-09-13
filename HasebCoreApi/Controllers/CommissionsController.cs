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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    [Authorize]
    public class CommissionsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public CommissionsController(IServiceWrapper serviceWrapper,IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }
        // GET: api/<CommissionsController>
        [HttpGet]
        public object Get([FromQuery] string branchId, DataSourceLoadOptions dataSource)
        {
            try
            {
                var data = _serviceWrapper.Commission.GetBranch(branchId);
                return DataSourceLoader.Load(data, dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET api/<CommissionsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var data = await _serviceWrapper.Commission.Get(id);
                return Ok(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST api/<CommissionsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var commissionTbl = new CommissionTbl();
            try
            {
                JsonConvert.PopulateObject(values, commissionTbl);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            //bank.UserId = User.GetUserId();

            if (!TryValidateModel(commissionTbl))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _bankBranch = await _serviceWrapper.Commission.Create(commissionTbl);
                return CreatedAtAction("Get", _bankBranch);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }

        // PUT api/<CommissionsController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromQuery] string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }

            var commissionTbl = await _serviceWrapper.Commission.Get(key);
            if (commissionTbl == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, commissionTbl);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(commissionTbl))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                commissionTbl.UpdateDate = DateTime.Now;
                commissionTbl.UpdaterId = User.GetUserId();
                var _commissionTbl = await _serviceWrapper.Commission.Update(commissionTbl);
                return Ok(_commissionTbl);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }

        // DELETE api/<CommissionsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
