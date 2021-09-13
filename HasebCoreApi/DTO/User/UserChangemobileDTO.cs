using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.DTO.User
{
    public class UserChangemobileDTO
    {
        [Required(ErrorMessage = "req_mobileOTP")]
        public string MobileOTP{ get; set; }
    }
}
