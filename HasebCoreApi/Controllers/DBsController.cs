//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using HasebCoreApi.Models;
//using Microsoft.AspNetCore.Authorization;
//using System;
//using System.Linq;
//using System.Security.Claims;
//using AutoMapper;
//using HasebCoreApi.DTO.DB;
//using System.Collections.Generic;
//using HasebCoreApi.Helpers;

//namespace HasebCoreApi.Controllers
//{
//    [Authorize]
//    [ApiController]
//    [Route("api/core/[controller]")]
//    public class DBsController : ControllerBase
//    {
//        private readonly IServiceWrapper _serviceWrapper;
//        private readonly IMapper _mapper;

//        public DBsController(IServiceWrapper serviceWrapper, IMapper mapper)
//        {
//            _serviceWrapper = serviceWrapper;
//            _mapper = mapper;
//        }
//        ///<summary>
//        ///  Getting That Specific User Databases List
//        /// </summary>
//        /// <returns></returns>
//        /// /// <remarks>
//        /// Sample Get:sfaafs
//        ///
//        /// GET /Get
//        /// {
//        ///     Null
//        /// }
//        ///
//        /// </remarks>
//        /// <param name="Get"></param>
//        [HttpGet("User")]
//        public async Task<ActionResult<Db>> GetDbs()
//        {
               
//            //Get list of Databases With User Id
//            var data = await _serviceWrapper.DatabaseService.GetListsByrole(User.GetUserId());
//            //Mapping Our List And Send Just Databases Name
//            var finalData = _mapper.Map<List<DbListDTO>>(data);
//            return Ok(finalData);
//        }
//        /// <returns></returns>
//        /// /// <remarks>
//        /// Sample Delete:
//        ///
//        ///     GET /Delete
//        ///     {
//        ///     Carefull from Delete 
//        ///        "id": "Number"
//        ///     }
//        ///
//        /// </remarks>
//        /// <param name="Delete"></param>
//        [HttpDelete("{id}")]
//        public async Task<ActionResult<Db>> DeleteDb(int id)
//        {
//            try
//            {
//                await _serviceWrapper.DatabaseService.DeleteDb(id);
//                return Ok();
//            }
//            catch (DbNotFoundException)
//            {
//                return BadRequest("Selected Database Not Found!");
//            }
//        }
//    }
//}