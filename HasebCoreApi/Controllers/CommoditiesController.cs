using System.Threading.Tasks;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HasebCoreApi.Helpers;
using Newtonsoft.Json;
using HasebCoreApi.DTO.Common;
using Microsoft.Extensions.Localization;
using HasebCoreApi.Localize;
using HasebCoreApi.Services.Commodities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using System;

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    [Authorize]
    public class CommoditiesController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer _localizer;

        public CommoditiesController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }
        [HttpGet("PriceFilter")]
        public async Task<object> GetCommo([FromQuery] bool IsCommodity, [FromQuery] string commodityGroupId)
        {
            try
            {
                var data = await _serviceWrapper.Commodity.GetbyCommo(IsCommodity, commodityGroupId);
                return data;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Get All Commodities 
        /// </summary>
        [HttpGet]
        public object Get([FromQuery] string branchId, [FromQuery] bool? IsCommodity, DataSourceLoadOptions dataSource)
        {

            try
            {
                if (IsCommodity.HasValue)
                {
                    var data = _serviceWrapper.Commodity.Get(branchId, IsCommodity);
                    return DataSourceLoader.Load(data, dataSource);
                }
                else
                {
                    var data = _serviceWrapper.Commodity.GetAll(branchId);
                    return DataSourceLoader.Load(data, dataSource);
                }
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }
        /// <summary>
        /// Get The Commodity With id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
            try
            {
                return Ok(await _serviceWrapper.Commodity.Get(id));
            }
            catch (CommodityNotFoundException)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }
        }

        [HttpGet("GetDiscountAndCommo")]
        public object GetCommoDiscount([FromQuery] string BranchId, [FromQuery] string PeriodId, [FromQuery] bool Iscommodity, DataSourceLoadOptions dataSource)
        {
            try
            {
                // var data = _serviceWrapper.Commodity.GetDiscountByPeriod(BranchId, PeriodId, Iscommodity);
                // return DataSourceLoader.Load(null, dataSource);
                return null;
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        /// <summary>
        ///     Create a commodity.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         IsCommodity:true/false => true => its commodity || false => its Service
        ///         BranchId : "5fa9043dd06b9463ce9cfeba",
        ///         IsSaleExempt : false,
        ///         IsPurchaseExempt : false,
        ///         CommodityCode : "varchar(15)",
        ///         NameOne : "Ajkjksadh",
        ///         TechnicalCode : "GFG555",
        ///         NameTwo : "Hakjasd",
        ///         SerialNumber : "78687ygg",
        ///         Barcode : "123123wadasd",
        ///         ServiceStuffId : "1231jhk",
        ///         TemplateCode : 1,
        ///         TemplateName : "Test123",
        ///         Photo:["Url1","url2"],
        ///         PurchaseDescription?:"Text",
        ///         SaleDescription?:"Text",
        ///         IsActive?:true,
        ///         UnitMainId : "5fa6532d0668649d7b89e261",
        ///         UnitSubId : "5fa651213b6822e4da517061",
        ///         UnitMainValue : 1,
        ///         UnitSubValue : 10,
        ///         CommodityGroupId : "5fa65ef2d435c2e8ac26fbb5",
        ///         Description?:"varchar(max)",
        ///         Price?:[{Year?:int , PeriodId?:string => 24length, Amount?:long}],
        ///         Discount?:[{Year?:int , PeriodId?:string => 24length, Percent?:int}]
        ///         Commission?:[{Percent:int , PeriodId:"24 length" , Year:int , Code:"varchar(max) ,NameOne:"varchar(max) , NameTwo:"varchar(max)" , IsActive?:Default => true}]
        ///         these field Need When its Commodity
        /// 
        ///         InitialInventory?:{Year?:int , PeriodId?:string => 24length, Amount?:long},
        ///         AvgInventory?:{Year?:int , PeriodId?:string => 24length, Amount?:long},
        ///         MinimumInventory?:long => it is like BigInt,
        ///         MaximumInventory?:long => it is like BigInt
        ///         
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Create is successfull</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "Branch not found." }
        ///     
        ///     { Code = 1, Message = "Commodity group not found." } 
        ///     
        ///     { Code = 2, Message = "Main unit is not found." } 
        ///     
        ///     { Code = 3, Message = "Sub unit is not found." } 
        ///     
        /// </response>

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values, List<IFormFile> files)
        {
            var commodity = new Commodity();

            try
            {
                commodity.UserId = User.GetUserId();

                JsonConvert.PopulateObject(values, commodity);

            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (commodity.IsCommodity != false)
            {
                if (commodity.AvgInventory == null ||
                    commodity.InitialInventory == null ||
                    commodity.MaximumInventory == 0 ||
                    commodity.MinimumInventory == 0
                    ) return BadRequest(new GenericMessage { Code = 4001, Message = _localizer.GetString("err_for_inventories") });
            }

            if (!TryValidateModel(commodity))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _commodity = await _serviceWrapper.Commodity.Create(commodity, files);
                return CreatedAtAction("Get", new { id = _commodity.Id }, _commodity);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
            catch (CommodityGroupNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_commodity_group_notFound") });
            }
            catch (UnitMainNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = _localizer.GetString("err_unit_main_notFound") });
            }
            catch (UnitSubNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 3, Message = _localizer.GetString("err_unit_sub_notFound") });
            }
            catch (ImageLengthSomuchException)
            {
                return BadRequest(new GenericMessage { Code = 4, Message = _localizer.GetString("err_image_length_somuch") });
            }
        }

        /// <summary>
        ///     Update a commodity.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "BranchId" : "5fa9043dd06b9463ce9cfeba",
        ///         "IsSaleExempt" : false,
        ///         "IsPurchaseExempt" : false,
        ///         "CommodityCode" : "321321",
        ///         "NameOne" : "Ajkjksadh",
        ///         "TechnicalCode" : "GFG555",
        ///         "NameTwo" : "Hakjasd",
        ///         "SerialNumber" : "78687ygg",
        ///         "Barcode" : "123123wadasd",
        ///         "ServiceStuffId" : "1231jhk",
        ///         "TemplateCode" : 1,
        ///         "TemplateName" : "Test123",
        ///         "Description" : "876214ghghjgagdk",
        ///         "UnitMainId" : "5fa6532d0668649d7b89e261",
        ///         "UnitSubId" : "5fa651213b6822e4da517061",
        ///         "UnitMainValue" : 1,
        ///         "UnitSubValue" : 10,
        ///         "CommodityGroupId" : "5fa65ef2d435c2e8ac26fbb5"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Create is successfull</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "Branch not found." }
        ///     
        ///     { Code = 1, Message = "Commodity group not found." } 
        ///     
        ///     { Code = 2, Message = "Main unit is not found." } 
        ///     
        ///     { Code = 3, Message = "Sub unit is not found." } 
        ///     
        /// </response>
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] string key, [FromForm] string values, List<IFormFile> files)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }

            var commodity = await _serviceWrapper.Commodity.GetById(key);
            if (commodity == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, commodity);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(commodity))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                commodity.UpdateDate = DateTime.Now;
                commodity.UpdaterId = User.GetUserId();
                var _commodity = await _serviceWrapper.Commodity.Update(commodity, files);
                return Ok(_commodity);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
            catch (CommodityGroupNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_commodity_group_notFound") });
            }
            catch (UnitMainNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = _localizer.GetString("err_unit_main_notFound") });
            }
            catch (UnitSubNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 3, Message = _localizer.GetString("err_unit_sub_notFound") });
            }
        }
        /// <summary>
        /// Delete For Images File
        /// </summary>
        /// <param name="FileNameAndPath"></param>
        /// <returns></returns>
        [HttpDelete("DeleteImage")]
        public async Task<IActionResult> DeleteFile([FromQuery] string FileNameAndPath)
        {
            try
            {
                await _serviceWrapper.Commodity.DeleteFile(FileNameAndPath);
                return Ok();
            }
            catch (FileNameOrPathNullException)
            {
                return BadRequest(new GenericMessage { Code = 3, Message = _localizer.GetString("err_file_path_null") });
            }

        }
        /// <summary>
        /// Delete The Commodities
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] string key)
        {
            try
            {

                await _serviceWrapper.Commodity.Delete(key);
                return Ok();

            }
            catch (FileNameOrPathNullException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_file_path_null") });
            }
            catch (IdLengthNotEqual)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("error_id_length_false") });
            }
            catch (CommodityReferencedToCommodityException)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = _localizer.GetString("err_commodity_has_referenced") });
            }
            catch (ImageDeleteException)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = _localizer.GetString("err_image_not_deleted") });
            }
        }
    }
}