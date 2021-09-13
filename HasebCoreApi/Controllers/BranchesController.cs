using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HasebCoreApi.DTO.Branch;
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
    public class BranchesController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer _localizer;

        public BranchesController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }

        // GET: api/<BranchesController>
        /// <summary>
        /// Get branches that related to the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("Dashboard")]
        public async Task<object> Get()
        {
            var userId = User.GetUserId();
            return await _serviceWrapper.Branch.GetDashboard(userId);
        }

        /// <summary>
        /// Get Branch With Id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            return Ok(await _serviceWrapper.Branch.Get(id));
        }
        /// <summary>
        /// Get User Info For Inserting With Id
        /// </summary>
        /// <remarks>
        /// 
        ///     {   
        ///     
        ///         Send The Mobile Number and Get {username, firstname , lastname , mobile ,userid}
        ///      
        ///     }
        ///     
        /// </remarks>

        [HttpGet("Getphone/{phonenumber}")]
        public async Task<IActionResult> Getphone(string phonenumber)
        {
            if (string.IsNullOrWhiteSpace(phonenumber))
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }

            try
            {
                var data = await _serviceWrapper.Branch.Getphone(phonenumber);
                return Ok(data);

            }
            catch (NoInformationNumberException)
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("error_no_mobile_found") });
            }
        }

        /// <summary>
        /// Insert New Branch with this api
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For authorization And Get UserId
        /// 
        /// 
        /// Sample request:
        ///
        /// 
        ///     {        
        /// 	        UserId?:[{UserId:"" , Title:""}], =>for now its will  be null
        ///	        OwenerId:"",
        ///	        BuyerId:"",
        ///	        CityId:"",
        ///	        PlanId:[""],
        ///	        GroupId?:"",
        ///	        ProductId:"",
        ///	        Name:" شعبه تبریز",
        ///	        StartDate:DateTime+duration,
        ///	        EndDate:DateTime+duration,
        ///	        IsActive?:default True,
        ///             TaxPayerId:"شناسه ثبت نام",
        ///	        AuthoritativeId?:"شناسه ملی شریک",
        ///	        RegistrationNumber?:"شماره ثبت",
        ///	        Economicode:"کد اقتصادی",
        ///	        Address:"آدرس"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Branch Create is successfull</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "Branch is duplicated" }
        ///     
        /// </response>
        [HttpPost]
        public async Task<ActionResult<Branch>> Post([FromForm] string values)
        {
            var branch = new Branch();
            try
            {
                JsonConvert.PopulateObject(values, branch);
                branch.ProductId = "5fb8f06083c64e1fe4d19212";
                branch.PlanId = new string[] { "5fb8ee72621785fe66f6bf36" };
                var plan = _serviceWrapper.Plan.Get("5fb8ee72621785fe66f6bf36");
                //                                                                          *******************FixMe*******************
                branch.StartDate = DateTime.Now;
                branch.EndDate = DateTime.Now.AddDays(plan.Result.Duration);
                if (branch.OwnerId == null)
                {
                    branch.OwnerId = User.GetUserId();
                    branch.BuyerId = User.GetUserId();
                }
                else
                {
                    branch.BuyerId = User.GetUserId();
                }
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(branch))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {

                await _serviceWrapper.Branch.Create(branch);
                return CreatedAtAction("Get", new { id = branch.Id }, branch);


            }
            catch (BranchDuplicateException ex)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_general_duplicate"), Data = ex.Branch });
            }
        }
        /// <summary>
        /// Update The Branch with this Api
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Send Jwt Token For authorzation
        /// 
        /// Send Specific Branch Id
        /// 
        ///     {
        ///	        ProductId:"",
        ///	        Name:""
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Update is successfull</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "Branch is duplicated" }
        ///     
        /// </response>
        [HttpPut]
        public async Task<ActionResult<Branch>> Put( string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            var branch = await _serviceWrapper.Branch.Get(key);
            if (branch == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, branch);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(branch))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                await _serviceWrapper.Branch.Update(branch);
                return Ok(branch);
            }
            catch (BranchDuplicateException ex)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_general_duplicate"), Data = ex.Branch });
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
