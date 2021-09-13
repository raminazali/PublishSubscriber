using System;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HasebCoreApi.DTO.Common;
using HasebCoreApi.Helpers;
using HasebCoreApi.Localize;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    [Authorize]
    public class BankAccountsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public BankAccountsController(IServiceWrapper serviceWrapper, IStringLocalizer<Resource> localizer)
        {
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }

        /// <summary>
        ///  Get All Bank Accounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetBranch([FromQuery] string branchId, DataSourceLoadOptions dataSource)
        {
            try
            {
                var data = _serviceWrapper.BankAccounts.GetBranch(branchId);
                return  DataSourceLoader.Load(data, dataSource);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }

        /// <summary>
        ///  Get Specific Bank Account With Id
        /// </summary>
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
                return Ok(await _serviceWrapper.BankAccounts.Get(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Insert Your New Bank Account
        /// </summary>
        /// <remarks>
        /// Send Token To Get UserId And Authorization
        /// 
        ///     {
        ///         BranchId:"24 Length",
        ///         CurrencyId:"24 Length",
        ///         BankBranchId:"24 Length",
        ///         Code:"varchar(max)",
        ///         AccountType:"varchar(max)",
        ///         CardNumber?:"varchar(max)"
        ///         IBAN:"varchar(max)"
        ///         AccountNameOne?:"varchar(max)",
        ///         AccountNameTwo?:"varchar(max)",
        ///         IsActive:"bool" => Default True
        ///     }
        /// 
        /// 
        /// </remarks>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var bankAccount = new BankAccount();

            try
            {
                bankAccount.UserId = User.GetUserId();
                JsonConvert.PopulateObject(values, bankAccount);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }


            if (!TryValidateModel(bankAccount))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                var _bankAccount = await _serviceWrapper.BankAccounts.Create(bankAccount);
                return CreatedAtAction("Get", new { id = _bankAccount.Id }, _bankAccount);
            }
            catch (BranchNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_branch_notFound") });
            }
        }

        /// <summary>
        /// Update Your Existing Bank Account
        /// </summary>
        /// <remarks>
        /// Send Token For Authorization
        /// 
        /// Send Which Field Is Updated
        /// 
        ///     {
        ///     
        ///         AccountNameOne?:"varchar(max)",
        ///         AccountNameTwo?:"varchar(max)",
        ///         AccountType:"varchar(max)",
        ///         CardNumber?:"varchar(max)"
        ///         
        ///     }
        /// 
        /// 
        /// </remarks>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] string key, [FromForm] string values)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length != 24)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }

            var bankAccount = await _serviceWrapper.BankAccounts.Get(key);
            if (bankAccount == null)
            {
                return NotFound(new GenericMessage { Code = 4004, Message = _localizer.GetString("err_record_not_found") });
            }

            try
            {
                JsonConvert.PopulateObject(values, bankAccount);
            }
            catch
            {
                return BadRequest(new GenericMessage { Code = 4000, Message = _localizer.GetString("err_format_not_valid") });
            }

            if (!TryValidateModel(bankAccount))
                return BadRequest(new GenericMessage { Code = 4001, Message = ModelState.GetError() });

            try
            {
                bankAccount.UpdateDate = DateTime.Now;
                bankAccount.UpdaterId = User.GetUserId();
                var _bank = await _serviceWrapper.BankAccounts.Update(bankAccount);
                return Ok(_bank);
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
                await _serviceWrapper.BankAccounts.Delete(key);
                return Ok();
            }
            catch (BankAccountReferencedException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_bankaccount_referenced") });
            }
            catch (IdLengthNotEqual)
            {
                return BadRequest(new GenericMessage { Code = 4002, Message = _localizer.GetString("error_id_length_false") });
            }
        }
    }
}
