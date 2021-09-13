using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("bank")]
    public class Bank : Document
    {
        [BsonElement("name")]
        [Required]
        public string Name { get; set; }
    }
}
