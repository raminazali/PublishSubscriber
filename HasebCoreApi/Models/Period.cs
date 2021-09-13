using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HasebCoreApi.Models
{
    [BsonCollection("period")]
    public class Period : Document
    {
        [BsonElement("branch_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string BranchId { get; set; }
        [Required]
        [BsonElement("from_date")]
        public DateTime FromDate { get; set; }
        [Required]
        [BsonElement("to_date")]
        public DateTime ToDate { get; set; }
        [Required]
        [BsonElement("type")]
        [StringLength(70, MinimumLength = 1)]
        public string Type { get; set; }
        [Required]
        [BsonElement("reference")]
        // Price = 1 | Discount = 2 | Pursant = 3
        public int Reference { get; set; }
        [BsonElement("is_buy")]
        public bool Isbuy { get; set; } = true;
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

        [BsonIgnore]
        public List<PeriodPrice> CommodityAmounts { get; set; }
        [BsonIgnore]
        public List<PeriodDiscount> PeriodDiscount { get; set; }
        [BsonIgnore]
        public List<PeriodCommission> PeriodCommission { get; set; }
    }

    public class PeriodPrice
    {
        [BsonIgnore]
        public string CommodityId { get; set; }
        [BsonIgnore]
        public long Amount { get; set; }
        [BsonIgnore]
        public int Year { get; set; }
    }
    public class PeriodDiscount
    {
        [BsonIgnore]
        public int Percent { get; set; }
        [BsonIgnore]
        public string CommodityId { get; set; }
        [BsonIgnore]
        public int Year { get; set; }
    }
    public class PeriodCommission
    {
        [BsonIgnore]
        public int Percent { get; set; }
        [BsonIgnore]
        public string CommodityId { get; set; }
        [BsonIgnore]
        public int Year { get; set; }
        [BsonIgnore]
        public string Code { get; set; }
        [BsonIgnore]
        public string NameOne { get; set; }
        [BsonIgnore]
        public string NameTwo { get; set; }
        [BsonIgnore]
        public bool IsActive { get; set; } = true;
    }
}
