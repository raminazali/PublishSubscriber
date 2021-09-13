using System;
using System.ComponentModel.DataAnnotations;

namespace HasebCoreApi.DTO
{
    public class UserAuthDTO
    {
        [Required(ErrorMessage = "req_username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "req_password")]
        public string Password { get; set; }

        //[RegularExpression(@"^(?:9\d{9})$", ErrorMessage = "err_mobile_validation")]
        //public string Mobile { get; set; }
    }
}