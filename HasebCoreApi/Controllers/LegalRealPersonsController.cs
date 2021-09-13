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
    public class LegalRealPersonsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer _localizer;
        public LegalRealPersonsController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }
        [HttpGet]
        public async Task<object> Get([FromQuery] string branchId , [FromQuery] bool isLegal, DataSourceLoadOptions dataSource)
        {
            try
            {
                var data = await _serviceWrapper.RealPerson.GetBranch(branchId, isLegal);
                return DataSourceLoader.Load(data , dataSource);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }

        /// <summary>
        /// Get Real Person With Id in This Api
        /// </summary>
        /// <remarks>
        /// Warn: Jwt Token As Authorize Header
        /// 
        /// Sample :
        ///     
        ///     {    
        ///         string id:"5fa9043dd06b9463ce9cfeba" => It Is the Real Person Id (RealPersonId)
        ///     }   
        ///     
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            try
            {
                var data = await _serviceWrapper.RealPerson.Get(id);
                data.Code.Remove(0);
                return Ok(data);
            }
            catch (NoPersonFoundException)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("error_no_realperson_found") });
            }

        }
        [HttpGet("{id}/RelatedPerson")]
        public async Task<IActionResult> GetRelatedPerson(string id)
        {
            try
            {
                return Ok(await _serviceWrapper.RealPerson.GetRelatedPerson(id));
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Insert Real Person With this Api
        /// </summary>
        /// <remarks>
        /// Warn: Jwt Token As Authorize Header
        /// 
        /// Sample :
        /// 
        ///     {        
        ///         BranchId:"5fa9043dd06b9463ce9cfeba",
        ///         Code:"varchar(max)",
        ///         NameOne:"varchar(200)",
        ///         LastNameOne:"varchar(200)",
        ///         NameTwo?:"varchar(200)",
        ///         LastNameTwo?:"varchar(200)"
        ///         EconomicCode?:"1131545",
        ///         DetailedGroup:["something0"],
        ///         TaxPayerType?:Default Value => عدم شمول ثبت نام
        ///         BuyerId?:"12321" => در شخص حقیقی منظور کد ملی شخص می باشد
        ///         RegistrationNumber?:"varchar(max)",
        ///         Email?:"Example@Gmail.com",
        ///         RelatedPerson:[{FirstName:"رامین" , LastName:"ازلی", Post:"Hello", Phone:"363534", Address:"یه جایی"}],
        ///         SystemAddress?:"varchar(max)" 
        ///         RelatedAddress:[{IsMain:"true or false => default false",Province:"varchar(70)",City:"varchar(70)",Address:"varchar(max)", BuyerPostalCode?:"varchar(10)"}],
        ///         IsLegal:bool => default true,
        ///         IsActive:bool => default true,
        ///         IsMale:bool => default true,
        ///         RelatedPhone :[{IsMain:"true or false => default false",IsPhone:"true or false => default true",PhoneNumber:"varchar(10)"}],
        ///     }
        ///     
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var realPerson = new LegalRealPerson();
            try
            {
                realPerson.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, realPerson);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(realPerson))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                await _serviceWrapper.RealPerson.Create(realPerson);
                return CreatedAtAction("Get", new { id = realPerson.Id }, realPerson);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Update Real Person With this Api
        /// </summary>
        /// <remarks>
        /// Warn: Jwt Token As Authorize Header
        /// 
        /// string id :"5fa9043dd06b9463ce9cfeba" => Send Id of the specific Real Person
        /// 
        /// Sample :
        /// 
        /// 
        ///     {        
        ///         
        ///         NameOne:"محمد"
        ///         Code:"1",
        ///         EconomicCode:"1131545",
        ///         DetailedGroup:["something0"],
        ///         BuyerId:"12321"
        ///     }
        ///     
        /// </remarks>
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            var realPerson = await _serviceWrapper.RealPerson.Get(key);
            try
            {
                JsonConvert.PopulateObject(values, realPerson);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(realPerson))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                realPerson.UpdateDate = DateTime.Now;
                realPerson.UpdaterId = User.GetUserId();
                await _serviceWrapper.RealPerson.Update(realPerson);
                return CreatedAtAction("Get", new { id = realPerson.Id }, realPerson);
            }
            catch
            {
                throw;
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] string key)
        {
            try
            {
                await _serviceWrapper.RealPerson.Delete(key);
                return Ok();
            }
            catch (PersonReferencedToinitialPersonException)
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_referenced_in_initialPerson_inventory") });
            }
            catch (IdLengthNotEqual)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("error_id_length_false") });
            }
        }
    }
}
