using Microsoft.AspNetCore.Mvc;
using Mongo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Publisher.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserControllers : ControllerBase
    {
        [HttpPost]
        public async Task Post()
        {
            Ok();
        }
    }
}
