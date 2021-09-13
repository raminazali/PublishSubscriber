using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HasebCoreApi.Models
{
    [BsonCollection("fixed_discount")]
    public class FixedDiscount : Document
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("branch_id")]
        [Required]
        public string BranchId { get; set; }
        [BsonElement("code")]
        [Required]
        public string Code { get; set; }
        [BsonElement("name_one")]
        public string NameOne { get; set; }
        [BsonElement("name_two")]
        public string NameTwo { get; set; }
        [BsonElement("percent")]
        public int? Percent { get; set; }
        [BsonElement("amount")]
        public long? Amount { get; set; }
        [BsonElement("is_active")]
        public bool IsActive { get; set; } = true;
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("user_id")]
        [Required]
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
