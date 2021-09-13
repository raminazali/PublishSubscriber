using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HasebCoreApi.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HasebCoreApi.Controllers
{
    [Route("api/core/[controller]")]
    [ApiController]
    public class NotificationHubController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        public NotificationHubController(IHubContext<NotificationHub> notificationHubContext)
        {
            _notificationHubContext = notificationHubContext;
        }

        //[HttpPost]
        //public async Task <ActionResult> Index()
        //{
        //    //await _notificationHubContext.Clients.All.s
        //}
    }
}
