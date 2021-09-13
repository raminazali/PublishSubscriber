using System;
using System.ComponentModel.DataAnnotations;

namespace HasebCoreApi.DTO
{
    public class UserAuthSuccDTO
    {
        public string Username { get; set; }
        public string Mobile { get; set; }
        public DateTime ServerDateTime { get; set; }
        public bool? IsActive { get; internal set; }
    }
}