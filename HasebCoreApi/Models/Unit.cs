using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HasebCoreApi.Models
{
    [BsonCollection("unit")]
    public class Unit : Document
    {
        [BsonElement("branch_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string BranchId { get; set; }
        [BsonElement("name_one")]
        [StringLength(45, MinimumLength = 1)]
        [Required]
        public string NameOne { get; set; }
        [BsonElement("name_two")]
        [StringLength(45, MinimumLength = 1)]
        public string NameTwo { get; set; }
        [BsonElement("unit_code")]
        [StringLength(15, MinimumLength = 1)]
        public string UnitCode { get; set; }
        [BsonElement("user_id")]
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;


    }
}