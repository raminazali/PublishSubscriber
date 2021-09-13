using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("legal_real_person")]
    public class LegalRealPerson : Document
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("branch_id")]
        [Required]
        public string BranchId { get; set; }
        [BsonElement("code")]
        [Required]
        public string Code { get; set; }
        [BsonElement("name_one")]
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string NameOne { get; set; }
        [BsonElement("last_name_one")]
        [StringLength(200, MinimumLength = 1)]
        public string LastNameOne { get; set; }
        [BsonElement("name_two")]
        [StringLength(200, MinimumLength = 1)]
        public string NameTwo { get; set; }
        [BsonElement("last_name_two")]
        [StringLength(200, MinimumLength = 1)]
        public string LastNameTwo { get; set; }
        [BsonElement("economic_code")]
        public string EconomicCode { get; set; }
        [BsonElement("detailed_group")]
        public string[] DetailedGroup { get; set; } = new string[] { };
        [BsonElement("taxpayer_type")]
        public string TaxPayerType { get; set; } = "عدم شمول ثبت نام";
        [StringLength(11, MinimumLength = 1)]
        [BsonElement("buyer_id")]
        public string BuyerId { get; set; }
        [BsonElement("registration_number")]
        public string RegistrationNumber { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("related_person")]
        public List<RelatedPerson> RelatedPerson { get; set; } = new List<RelatedPerson> { };
        [BsonElement("system_address")]
        public string SystemAddress { get; set; }
        [BsonElement("related_address")]
        public List<RelatedAddress> RelatedAddress { get; set; } = new List<RelatedAddress> { };
        [BsonElement("is_legal")]
        public bool IsLegal { get; set; } = true;
        [BsonElement("is_active")]
        public bool IsActive { get; set; } = true;
        [BsonElement("is_male")]
        public bool IsMale { get; set; } = true;
        [BsonElement("related_phone")]
        public List<RelatedPhone> RelatedPhone { get; set; } = new List<RelatedPhone> { };
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("user_id")]
        public string UserId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("updater_id")]
        public string UpdaterId { get; set; }
        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [BsonElement("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}
