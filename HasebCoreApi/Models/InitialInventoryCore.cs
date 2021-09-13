using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("initial_inventory_core")]
    public class InitialInventoryCore : Document
    {
        [BsonElement("branch_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string BranchId { get; set; }
        [BsonElement("date")]
        [Required]
        public DateTime? Date { get; set; }
        [BsonElement("year")]
        [Required]
        public int? Year { get; set; }
        [BsonElement("reference")]
        [Required]
        // 1 => person | 2 => Bank  | 3 => pos | 4 => cash desk
        public int Reference { get; set; }
        [BsonElement("number")]
        [Required]
        public int Number { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("is_base_currency")]
        public bool IsBaseCurrency { get; set; } = false;
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string UserId { get; set; }
        [BsonElement("updater_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UpdaterId { get; set; }
        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [BsonElement("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}
