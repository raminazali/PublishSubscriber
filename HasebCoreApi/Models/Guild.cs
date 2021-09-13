using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HasebCoreApi.Models
{
    [BsonCollection("guild")]
    public partial class Guild : Document
    {
        [BsonElement("guild_name")]
        [StringLength(60, MinimumLength = 1)]
        [Required]
        public string GuildName { get; set; }
        [BsonElement("sub_to_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SubToId { get; set; }
    }
}
