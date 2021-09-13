using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("currency")]
    public class Currency : Document
    {
        [BsonElement("name_one")]
        [Required]
        public string NameOne { get; set; }
        [BsonElement("name_two")]
        public string NameTwo { get; set; }
        [BsonElement("abbreviation")]
        [Required]
        public string Abbreviation { get; set; }
        [Required]
        [BsonElement("foreign_exchange_code")]
        [Range(1,3)]
        public int ForeignExchangeCode { get; set; }
    }
}
