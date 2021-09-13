using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("detailed_group")]
    public class DetailedGroup: Document
    {
        [Required]
        [StringLength(45, MinimumLength = 1)]
        [BsonElement("name")]
        public string Name { get; set; }
    }
}
