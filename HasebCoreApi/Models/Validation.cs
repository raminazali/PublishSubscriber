using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HasebCoreApi.Models
{
    [BsonCollection("validation")]
    public partial class Validation : Document
    {
        [Required]
        [BsonElement("token")]
        [StringLength(10, MinimumLength = 1)]
        public string Token { get; set; }
        [BsonElement("count")]
        public short? Count { get; set; } = 0;
        [BsonElement("type")]
        public bool? Type { get; set; } = true;
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        [BsonElement("user_id")]
        public string UserId { get; set; }
        [Required]
        [BsonElement("create_date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
