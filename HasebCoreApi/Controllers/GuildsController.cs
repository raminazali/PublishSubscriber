using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.DTO.Common;
using HasebCoreApi.Helpers;
using HasebCoreApi.Localize;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    public class GuildsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public GuildsController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var data = await _serviceWrapper.Guild.Get();
                return data;
            }
            catch (GuildListEmptyException ex)
            {
                throw ex;
                // return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("plan_notfound") });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            try
            {
                return Ok(await _serviceWrapper.Guild.Get(id));
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var guild = new Guild();
            if (guild == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }
            try
            {
                JsonConvert.PopulateObject(values, guild);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(guild))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });
            try
            {
                await _serviceWrapper.Guild.Create(guild);
                return CreatedAtAction("Get", new { id = guild.Id }, guild);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromForm]string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            var guild = await _serviceWrapper.Guild.Get(key);
            if (guild == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, guild);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(guild))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                await _serviceWrapper.Guild.Update(guild);
                return Ok(guild);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        // DELETE api/<CitiesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok("Delete Not Completed");
        }
    }
}