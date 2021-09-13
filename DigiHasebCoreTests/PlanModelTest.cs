using HasebCoreApi;
using HasebCoreApi.Controllers;
using HasebCoreApi.Models;
using HasebCoreApi.Services.Plans;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DigiHasebCoreTests
{
    public class PlanModelTest
    {
        private readonly IServiceWrapper _serviceWrapper;
        public PlanModelTest(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }
        //[Fact]
        //public Task PlanService_GetTest()
        //{
        //   // var controller = new PlansController(_serviceWrapper);
        //   // var response = controller.Get();
        //   // var plan = new List<Plan>{ 
        //   // new Plan {Name ="123", Duration=1 },
        //   // new Plan {Name ="asgsgas", Duration=2 },
        //   // new Plan {Name ="12asgasga3", Duration=4 },
        //   // };
        //   //await Assert.IsType<Task<IActionResult>>(response);
        //}
    }
}



