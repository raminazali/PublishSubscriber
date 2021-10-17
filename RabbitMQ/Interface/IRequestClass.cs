using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Interface
{
    public interface IRequestClass
    {
        void Sender(string request);
    }
}
