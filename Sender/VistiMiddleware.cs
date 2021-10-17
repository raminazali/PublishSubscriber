using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using RabbitMQ.Interface;
using RabbitMQ.Models;
using System.Text;

namespace Sender
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class VistiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRequestClass _requestClass;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VistiMiddleware(RequestDelegate next, IRequestClass requestClass, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _requestClass = requestClass;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var header = _httpContextAccessor.HttpContext.Request.Headers.Select(q => q).ToDictionary(q => q.Key, q => q.Value.ToString());
            var ip = _httpContextAccessor.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var path = _httpContextAccessor.HttpContext.Request.Path.ToString();
            var body = _httpContextAccessor.HttpContext.Request.Body;
            var _body = String.Empty;
            if (body.CanRead)
            {
                var stream = new StreamReader(body, Encoding.UTF8, true, 1024, true);
                _body = await stream.ReadToEndAsync();

            }

            var model = new Request();
            // Manually Handle API's 
            if (path == "/api/UserControllers")
            {

                model.Body = _body;
                model.OperationId = 2;
                model.Connection = ip;
                model.Path = path;
                model.Header = header;
            } else
            {

                model.Body = _body;
                model.OperationId = 1;
                model.Connection = ip;
                model.Path = path;
                model.Header = header;
            }


            var json = JsonSerializer.Serialize(model);

            _requestClass.Sender(json);

            await _next(httpContext);
        }
    }
}
