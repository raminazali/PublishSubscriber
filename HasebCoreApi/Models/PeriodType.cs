using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("periode_type")]
    public class PeriodType : Document
    {
        [BsonElement("branch_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BranchId { get; set; }
        [BsonElement("name")]
        [StringLength(70, MinimumLength = 1)]
        [Required]
        public string Name { get; set; }
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string UserId { get; set; }
        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
