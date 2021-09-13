using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("currency_definition")]
    public class CurrencyDefinition : Document
    {
        [BsonElement("currency_id")]
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CurrencyId { get; set; }

        [BsonElement("branch_id")]
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BranchId { get; set; }

        [Required]
        [BsonElement("base_rate")]
        public long BaseRate { get; set; }

        [Required]
        [BsonElement("is_active")]
        public bool IsActive { get; set; } = true;

        [BsonElement("user_id")]
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
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
