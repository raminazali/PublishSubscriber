using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.DTO.User
{
    public class ForgetPasswordMobileDTO
    {
        [Required(ErrorMessage ="req_mobile")]
        public string Mobile { get; set; }
    }
}
