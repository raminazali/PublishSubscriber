using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.DTO.User
{
    public class UserEmailDTO
    {
        [Required(ErrorMessage = "req_email")]
        public string Email { get; set; }
    }
}
