using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HasebCoreApi.Models
{
    [BsonCollection("branch")]
    public partial class Branch : Document
    {
        [BsonElement("user_id")]
        public List<BranchUser> UserId { get; set; } = new List<BranchUser> { };

        [BsonElement("owner_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string OwnerId { get; set; }

        [BsonElement("buyer_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string BuyerId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("city_id")]
        [Required]
        public string CityId { get; set; }

        [BsonElement("plan_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string[] PlanId { get; set; }

        [BsonElement("group_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string GroupId { get; set; }

        [BsonElement("product_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string ProductId { get; set; }

        [BsonElement("name")]
        [StringLength(70, MinimumLength = 1)]
        [Required]
        public string Name { get; set; }

        [BsonElement("start_date")]
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        [BsonElement("end_date")]
        public DateTime EndDate { get; set; }

        [BsonElement("is_active")]
        public bool IsActive { get; set; } = true;

        [BsonElement("tax_payer_id")]
        [StringLength(14, MinimumLength = 1)]
        [Required]
        public string TaxPayerId { get; set; }

        [BsonElement("authoritative_id")]
        [StringLength(14, MinimumLength = 1)]
        public string AuthoritativeId { get; set; }
        [BsonElement("registration_number")]
        [StringLength(50, MinimumLength = 1)]
        public string RegistrationNumber { get; set; }
        [BsonElement("Economi_code")]
        [StringLength(50, MinimumLength = 1)]
        public string Economicode { get; set; }
        [BsonElement("address")]
        [Required]
        public string Address { get; set; }
    }

    public class BranchUser
    {
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }
    }
}
