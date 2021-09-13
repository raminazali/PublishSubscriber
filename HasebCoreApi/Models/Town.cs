using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HasebCoreApi.Models
{
    [BsonCollection("town")]
    public partial class Town : Document
    {
        [BsonElement("province_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string ProvinceId { get; set; }
        [BsonElement("sub_to_id")]
        public int? SubToId { get; set; }
        [BsonElement("name")]
        [StringLength(45, MinimumLength = 1)]
        [Required]
        public string Name { get; set; }
    }
}
