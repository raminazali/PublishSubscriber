using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HasebCoreApi.Models
{
    [BsonCollection("commission_tbl")]
    public class CommissionTbl : Document
    {
        [BsonElement("branch_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string BranchId { get; set; }
        [Required]
        [BsonElement("code")]
        public string Code { get; set; }
        [BsonElement("name_one")]
        [Required]
        public string NameOne { get; set; }
        [BsonElement("name_two")]
        public string NameTwo { get; set; }
        [BsonElement("from_date")]
        [Required]
        public DateTime FromDate{ get; set; }
        [BsonElement("to_date")]
        [Required]
        public DateTime ToDate { get; set; }
        [BsonElement("is_simple")]
        public bool IsSimple { get; set; } = true;
        [BsonElement("is_calculate")]
        public bool IsCalcilate { get; set; } = true;
        [BsonElement("is_invoice")]
        public bool IsInvoice { get; set; } = true;
        [BsonElement("is_active")]
        public bool IsActive { get; set; } = true;
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string UserId { get; set; }
        [BsonElement("updater_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UpdaterId { get; set; }
        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [BsonElement("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}
