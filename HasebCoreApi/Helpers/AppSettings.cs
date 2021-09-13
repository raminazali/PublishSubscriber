using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;

namespace HasebCoreApi
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string DigiAddress { get; set; }
        public double OTPRemainTime { get; set; }
    }
}
