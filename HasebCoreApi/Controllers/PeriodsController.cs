using System.Threading.Tasks;
using HasebCoreApi.DTO.Common;
using HasebCoreApi.Helpers;
using HasebCoreApi.Localize;
using HasebCoreApi.Models;
using HasebCoreApi.Services.Periods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    [Authorize]
    public class PeriodsController : Controller
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public PeriodsController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer;
            _serviceWrapper = serviceWrapper;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="commodityId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Get([FromQuery] string branchId, [FromQuery] string commodityId)
        {
            try
            {
                return await _serviceWrapper.PricePeriod.GetCommodityPricePeriods(branchId, commodityId);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        /// <summary>
        /// Get EndDate By With This Api 
        /// </summary>
        /// <remarks>
        /// 
        ///     {
        ///         BranchId:24 Length,
        ///         Type:string,
        ///         Reference: 1=> price , 2=> Discount , 3 => Commission
        ///         Isbuy:true or false
        ///     }
        ///     
        /// </remarks>
        /// <param name="branchId"></param>
        /// <param name="type"></param>
        /// <param name="reference"></param>
        /// <param name="Isbuy"></param>
        /// <returns></returns>
        [HttpGet("GetByType")]
        public async Task<object> GetByReference([FromQuery] string branchId, [FromQuery] string type, [FromQuery] int reference, [FromQuery] bool Isbuy)
        {
            try
            {
                return await _serviceWrapper.PricePeriod.GetByType(branchId, type, reference, Isbuy);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Send BranchId For Get Discount Type
        /// 
        /// 
        ///     {
        ///         Get Discount Types 
        ///         [FromQuery] string Branch Id
        ///         [FromQuery] bool IsBuy
        ///     }
        /// 
        /// </remarks>
        /// <param name="branchId"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        [HttpGet("GetListDiscount")]
        public object GetlistDiscount([FromQuery] string branchId, [FromQuery] int reference, [FromQuery] bool Isbuy, DataSourceLoadOptions dataSource)
        {
            try
            {
                var data = _serviceWrapper.PricePeriod.GetQueryable(branchId, reference, Isbuy);
                return DataSourceLoader.Load(data, dataSource);
                //return data;
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        /// <summary>
        ///     Create a Price Period.
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For UserId
        /// Sample request:
        ///         
        ///     {   
        ///         BranchId:"5fa9043dd06b9463ce9cfeba",
        ///         FromDate: => Send Date ad,
        ///         ToDate: => Send Date ad,
        ///         Type:"Varchar(70)",
        ///         IsBuy: Default true ,
        ///         CommodityAmotnts : [{"CommodityId" : "5fafc8ae2752af313c675f52",  "Amount" : 50000, Year:1399 or 1400}]
        ///     } 
        ///
        /// </remarks>
        /// <response code="200">Create is successfull</response>

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] string values)
        {
            var pricePeriod = new Period();
            try
            {
                pricePeriod.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, pricePeriod);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(pricePeriod))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _pricePeriod = await _serviceWrapper.PricePeriod.Create(pricePeriod);
                return CreatedAtAction("Get", new { id = _pricePeriod.Id }, _pricePeriod);
            }
            catch (CommodityNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_commodity_notFound") });
            }
            catch (CommodityPricePeriodDuplicateException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_commodity_price_period_duplicate") });
            }
            catch (PricePeriodOverlapException ex)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = _localizer.GetString("err_price_period_overlap"), Data = ex.PricePeriod });
            }
        }
        /// <summary>
        ///     Create a Discount Period.
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For UserId
        /// Sample request:
        ///         
        ///     {   
        ///         BranchId:"5fa9043dd06b9463ce9cfeba",
        ///         FromDate: => Send Date ad,
        ///         ToDate: => Send Date ad,
        ///         Type:"Varchar(70)",
        ///         IsBuy: Default true ,
        ///         PeriodDiscount : [{"CommodityId" : "5fafc8ae2752af313c675f52",  "Percent" : 50 , Year:1399 or 1400}]
        ///     } 
        ///
        /// </remarks>
        /// <response code="200">Create is successfull</response>

        [HttpPost("CreateDiscount")]
        public async Task<IActionResult> CreateDiscount([FromForm] string values)
        {
            var discount = new Period();
            try
            {
                discount.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, discount);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(discount))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _pricePeriod = await _serviceWrapper.PricePeriod.CreateDiscount(discount);
                return CreatedAtAction("Get", new { id = _pricePeriod.Id }, _pricePeriod);
            }
            catch (CommodityNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_commodity_notFound") });
            }
            catch (CommodityPricePeriodDuplicateException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_commodity_price_period_duplicate") });
            }
            catch (PricePeriodOverlapException ex)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = _localizer.GetString("err_price_period_overlap"), Data = ex.PricePeriod });
            }
        }
        /// <summary>
        ///     Create a Commission Period.
        /// </summary>
        /// <remarks>
        /// Send Jwt Token For UserId
        /// Sample request:
        ///         
        ///     {   
        ///         BranchId:"5fa9043dd06b9463ce9cfeba",
        ///         FromDate: => Send Date ad,
        ///         ToDate: => Send Date ad,
        ///         Type:"Varchar(70)",
        ///         PeriodCommission : [{Percent:int ,  ,"CommodityId" : "5fafc8ae2752af313c675f52", Year:1399 or 1400 , Code:string , NameOne:string , NameTwo:string, IsActive:Default true}]
        ///     } 
        ///
        /// </remarks>
        /// <response code="200">Create is successfull</response>
        [HttpPost("CreateCommission")]
        public async Task<IActionResult> CreateCommission([FromForm] string values)
        {
            var commission = new Period();
            try
            {
                commission.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, commission);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(commission))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _pricePeriod = await _serviceWrapper.PricePeriod.Createcommission(commission);
                return CreatedAtAction("Get", new { id = _pricePeriod.Id }, _pricePeriod);
            }
            catch (CommodityNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_commodity_notFound") });
            }
            catch (CommodityPricePeriodDuplicateException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_commodity_price_period_duplicate") });
            }
            catch (PricePeriodOverlapException ex)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = _localizer.GetString("err_price_period_overlap"), Data = ex.PricePeriod });
            }
        }
    }
}
