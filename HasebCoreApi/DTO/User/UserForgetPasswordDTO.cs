using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.DTO.User
{
    public class UserForgetPasswordDTO
    {
        [Required(ErrorMessage = "req_identifier")]
        public string Identifier { get; set; }
    }
}
