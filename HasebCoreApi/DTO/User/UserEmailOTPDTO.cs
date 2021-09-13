using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.DTO.User
{
    public class UserEmailOTPDTO
    {
        [Required(ErrorMessage ="req_emailOTP")]
        public string EmailOTP { get; set; }
    }
}
