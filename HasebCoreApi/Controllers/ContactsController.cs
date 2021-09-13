using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HasebCoreApi.DTO.Common;
using HasebCoreApi.DTO.ContactUs;
using HasebCoreApi.Helpers;
using HasebCoreApi.Localize;
using HasebCoreApi.Models;
using HasebCoreApi.Services.ContactUs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using static HasebCoreApi.Models.Contactus;

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    public class ContactsController : Controller
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public ContactsController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer;
            _serviceWrapper = serviceWrapper;
        }

        /// <summary>
        ///     Create a contact us.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///         
        ///     {
        ///         "Name" : "wqwewq",
        ///         "Email" : "wqwewq@gmail.com",
        ///         "Subject" : "test 123",
        ///         "Text" : "test 123 2eqweqweqwe"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Create is successfull</response>

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromForm] string values)
        {
            var contactus = new Contactus();
            try
            {
                JsonConvert.PopulateObject(values, contactus);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(contactus))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            var _contact = await _serviceWrapper.Contact.CreateContact(contactus);
            return CreatedAtAction("Get", new { id = _contact.Id }, _contact);

        }

        /// <summary>
        ///     Create a answer for contact us.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///         
        ///     {
        ///         "Text" : "test 123 2eqweqweqwe"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Create is successfull</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "Contact us not found." }
        ///
        /// </response>

        [HttpPost("{id}/Answer")]
        public async Task<IActionResult> AnswerContact(string id, [FromForm] string values)
        {
            var contactusAnswer = new ContactusAnswer();
            try
            {
                contactusAnswer.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, contactusAnswer);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(contactusAnswer))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _conatct = await _serviceWrapper.Contact.AnswerContact(id, contactusAnswer);
                return Ok(_conatct);
            }
            catch (ContactNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_contact_notFound") });
            }
        }
        // DELETE api/<CitiesController>/5
        [HttpDelete]
        public  async Task<IActionResult> Delete([FromForm] string key)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            try
            {
                await _serviceWrapper.Contact.Delete(key);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
