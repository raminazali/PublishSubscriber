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
    public class PosController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public PosController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> Localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = Localizer;
        }
        /// <summary>
        ///  Get Poses List
        /// </summary>
        /// <remarks>
        ///     
        ///     {
        ///         Send Jwt Token For Authorization
        ///     }
        /// 
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public object Get([FromQuery] string branchId, DataSourceLoadOptions dataSource)
        {
            try
            {
                var data = _serviceWrapper.Pos.GetBranch(branchId);
                return DataSourceLoader.Load(data, dataSource);
                //return data;
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        /// <summary>
        ///  Insert New Pos 
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For UserId And Authorization
        /// 
        ///     {
        ///     
        ///         Send Id And Get Pos
        ///         
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
                return Ok(await _serviceWrapper.Pos.Get(id));
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        ///  Insert New Pos 
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For UserId And Authorization
        /// 
        ///     {
        ///     
        ///         BranchId:"",
        ///         CurrencyId:"",
        ///         BankAccount:"",
        ///         Code:"varchar(max)",
        ///         PostNameOne:"varchar(max)",
        ///         PostNameTwo:"varchar(max)",
        ///         DeviceConnect?:"varchar(max)",
        ///         TerminalNumber:"varchar(max)",
        ///         IsActive:true => by Default its True You can Not Send
        ///         
        ///     }
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var pos = new Pos();

            try
            {
                pos.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, pos);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }


            if (!TryValidateModel(pos))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _pos = await _serviceWrapper.Pos.Create(pos);
                return CreatedAtAction("Get", new { id = _pos.Id }, _pos);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
            catch (CurrencyNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_currency_not_found") });
            }
            catch (BankAccountNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = _localizer.GetString("err_bank_account_not_found") });
            }
        }
        /// <summary>
        ///  Update Pos 
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For UserId And Authorization
        /// 
        ///     {
        ///     
        ///         Send That field You want to Update
        ///         
        ///     }
        /// 
        /// </remarks>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }

            var pos = await _serviceWrapper.Pos.Get(key);
            if (pos == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, pos);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(pos))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                pos.UpdateDate = DateTime.Now;
                pos.UpdaterId = User.GetUserId();
                var _pos = await _serviceWrapper.Pos.Update(pos);
                return Ok(_pos);
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
                await _serviceWrapper.Pos.Delete(key);
                return Ok();
            }
            catch (ReferencedToInitialPosException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_initial_pos_referenced") });
            }
            catch (IdLengthNotEqual)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("error_id_length_false") });
            }
        }
    }
}
