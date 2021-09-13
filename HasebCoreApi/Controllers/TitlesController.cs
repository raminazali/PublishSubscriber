using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.DTO.Common;
using HasebCoreApi.Helpers;
using HasebCoreApi.Localize;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    public class TitlesController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer _localizer;

        public TitlesController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<List<Title>> Get()
        {
            return await _serviceWrapper.Title.Get();
        }

        /// <summary>
        /// Get a title by Id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            return Ok(await _serviceWrapper.Title.Get(id));
        }

        /// <summary>
        /// Create a new title
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "Name": "ali",
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Creation is successfull</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "Title is duplicated" }
        ///     
        /// </response>
        [HttpPost]
        public async Task<ActionResult<Title>> Post([FromForm] string values)
        {
            var title = new Title();
            try
            {
                JsonConvert.PopulateObject(values, title);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(title))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _title = await _serviceWrapper.Title.Create(title);
                return CreatedAtAction("Get",new {id = title.Id }, title);
            }
            catch (TitleDuplicateException ex)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_general_duplicate"), Data = ex.Title });
            }
        }

        /// <summary>
        /// Update a new title
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "Name": "ali",
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Update is successfull</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "Title is duplicated" }
        ///     
        /// </response>
        [HttpPut("{id}")]
        public async Task<ActionResult<Title>> Put(string id,[FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            var title = await _serviceWrapper.Title.Get(id);
            if (title == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, title);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(title))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                await _serviceWrapper.Title.Update(title);
                return Ok(title);
            }
            catch (TitleDuplicateException ex)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_general_duplicate"), Data = ex.Title });
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
