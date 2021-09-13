using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HasebCoreApi.Models
{

    [BsonCollection("user")]
    public partial class UserInfo : Document
    {
        //[Required]
        [BsonElement("user_name")]
        [StringLength(70)]
        [Required]
        public string UserName { get; set; }
        //[Required]
        [BsonElement("password")]
        [StringLength(100)]
        [Required]
        public string Password { get; set; }
        //[Required]
        [BsonElement("mobile")]
        [StringLength(10)]
        [Required]
        public string Mobile { get; set; }
        [BsonElement("mobile_validator")]
        public bool MobileValidate { get; set; } = false;
        [BsonElement("Email")]
        [StringLength(100, MinimumLength = 1)]
        public string Email { get; set; }
        [BsonElement("email_validator")]
        public bool EmailValidate { get; set; } = false;
        [BsonElement("is_active")]
        public bool IsActive { get; set; } = false;
        [BsonElement("first_name")]
        [StringLength(70, MinimumLength = 1)]

        public string FirstName { get; set; }
        [BsonElement("last_name")]
        [StringLength(70, MinimumLength = 1)]
        public string LastName { get; set; }
        [BsonElement("national_number")]
        [StringLength(10, MinimumLength = 1)]
        public string NationalNumber { get; set; } = null;
        [BsonElement("postal_code")]
        [StringLength(10, MinimumLength = 1)]
        public string PostalCode { get; set; }
        [BsonElement("create_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [BsonElement("token")]
        public string Token { get; set; }

        [BsonElement("token_create_date")]
        public DateTime? TokenExpireDate { get; set; }

    }
}
