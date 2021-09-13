using System;
using System.ComponentModel.DataAnnotations;

namespace HasebCoreApi.DTO
{
    public class UserSignupDTO
    {
        [Required(ErrorMessage = "req_username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "req_password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "req_mobile")]
        [RegularExpression(@"^9\d{9}$" , ErrorMessage = "err_mobile_validation")]
        public string Mobile { get; set; }
    }
}