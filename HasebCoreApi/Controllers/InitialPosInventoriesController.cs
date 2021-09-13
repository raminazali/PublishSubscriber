﻿using System;
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
    public class InitialPosInventoriesController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public InitialPosInventoriesController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }
        [HttpGet]
        public object GetBranch([FromQuery] string BranchId, DataSourceLoadOptions dataSource)
        {
            try
            {
                var data = _serviceWrapper.InitialPosInventory.GetBranch(BranchId);
                return DataSourceLoader.Load(data, dataSource);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var data = await _serviceWrapper.InitialPosInventory.Get(id);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var initialPosInventory = new InitialPosInventory();

            try
            {
                JsonConvert.PopulateObject(values, initialPosInventory);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }


            if (!TryValidateModel(initialPosInventory))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _initialPosInventory = await _serviceWrapper.InitialPosInventory.Create(initialPosInventory);
                return CreatedAtAction("Get", new { id = _initialPosInventory.Id }, _initialPosInventory);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }

            var initialPosInventory = await _serviceWrapper.InitialPosInventory.Get(key);
            if (initialPosInventory == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, initialPosInventory);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(initialPosInventory))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _initialPosInventory = await _serviceWrapper.InitialPosInventory.Update(initialPosInventory);
                return Ok(_initialPosInventory);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }

        [HttpDelete]
        public async Task<IActionResult>  Delete([FromForm] string key)
        {
            try
            {
                await _serviceWrapper.InitialPosInventory.Delete(key);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
