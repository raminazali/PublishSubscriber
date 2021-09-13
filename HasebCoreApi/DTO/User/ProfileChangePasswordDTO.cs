using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.DTO.User
{
    public class ProfileChangePasswordDTO
    {
        [Required(ErrorMessage ="req_newPassword")]       
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "req_oldPassword")]
        public string OldPassword { get; set; }
    }
}
