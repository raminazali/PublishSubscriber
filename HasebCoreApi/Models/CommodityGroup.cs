using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations.Schema;

namespace HasebCoreApi.Models
{
    [BsonCollection("commodity_group")]
    public class CommodityGroup : Document
    {
        [BsonElement("sub_to_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [LengthIfNotNull(24 ,ErrorMessage = "err_objectId_format")] 
        public string SubToId { get; set; }

        [BsonElement("branch_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string BranchId { get; set; }

        [BsonElement("level")]
        [Required]
        public int Level { get; set; }

        [BsonElement("code")]
        [StringLength(45, MinimumLength = 1)]
        [Required]
        public string Code { get; set; }

        [BsonElement("name")]
        [StringLength(45, MinimumLength = 1)]
        [Required]
        public string Name { get; set; }

        [BsonElement("user_id")]
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}