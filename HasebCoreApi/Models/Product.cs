using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HasebCoreApi.Models
{
    [BsonCollection("product")]

    public partial class Product : Document
    {
        [BsonElement("module_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string[] Modules { get; set; } = new string[] { };
        [Required]
        [BsonElement("guild_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string GuildId { get; set; }
        [BsonElement("name")]
        [StringLength(45, MinimumLength = 1)]
        [Required]
        public string Name { get; set; }
    }
}
