using System.ComponentModel.DataAnnotations;

namespace HasebCoreApi.DTO.User
{
    public class ChangeMobileDTO
    {
        [Required(ErrorMessage = "req_mobileOTP")]
        public string MobileOTP { get; set; }
    }
}
