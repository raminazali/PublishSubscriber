using System;
using System.ComponentModel.DataAnnotations;

namespace HasebCoreApi.DTO
{
    public class UserOTPDTO
    {
        [Required(ErrorMessage = "req_username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "req_otp")]
        public string OTP { get; set; }
        public bool Signup { get; set; }
        public bool ForgetPassword { get; set; }
        public bool IsActive { get; set; }
    }
}