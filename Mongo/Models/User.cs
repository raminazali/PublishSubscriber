using System;
using System.Collections.Generic;
using System.Text;

namespace Mongo.Models
{
    public class User
    {
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
    }
}
