using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQ.Models
{
    public class Request
    {
        public string Path { get; set; }
        public string Connection { get; set; }
        public string Body { get; set; }
        public int OperationId  { get; set; }
        public Dictionary<string, string> Header { get; set; }
    }
}
