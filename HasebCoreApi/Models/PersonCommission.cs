using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("person_commission")]
    public class PersonCommission : Document
    {
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("commission_id")]
        public string CommissionId { get; set; }
        [BsonElement("person_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string PersonId { get; set; }
        [BsonElement("from_amount")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public long FromAmount { get; set; }
        [BsonElement("to_amount")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public long ToAmount { get; set; }
        [BsonElement("percent")]
        [Required]
        public int Percent { get; set; }
    }
}
