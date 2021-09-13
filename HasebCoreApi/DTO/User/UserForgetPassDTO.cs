using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.DTO.User
{
    public class UserForgetPassDTO
    {
        [Required(ErrorMessage = "req_otp")]
        [StringLength(5)]
        public string OTP { get; set; }

        [Required(ErrorMessage = "req_username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "req_password")]
        public string Password { get; set; }
    }
}
