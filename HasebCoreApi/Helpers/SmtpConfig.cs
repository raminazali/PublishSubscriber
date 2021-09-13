using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Helpers
{
    public class SmtpConfig
    {
        public int SmtpPort { get; set; }
        public string From { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpUser { get; set; }
    }
}
