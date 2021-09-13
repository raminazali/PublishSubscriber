using System;
using System.ComponentModel.DataAnnotations;

namespace HasebCoreApi.DTO
{
    public class UserInfoDTO
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime ServerDateTime { get; set; }
    }
}