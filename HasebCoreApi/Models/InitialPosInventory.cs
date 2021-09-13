using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("initial_pos_inventory")]
    public class InitialPosInventory: Document
    {
        [BsonElement("pos_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string PosId { get; set; }
        [BsonElement("initial_inventory")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string InitialInventory { get; set; }
        [BsonElement("amount")]
        [Required]
        public long Amount { get; set; }
        [BsonElement("currency_amount")]
        public long? CurrencyAmount { get; set; }
        [BsonElement("currency_rate")]
        public long? CurrencyRate { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
    }
}
