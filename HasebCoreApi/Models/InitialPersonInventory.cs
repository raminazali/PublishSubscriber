﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("initial_person_inventory")]
    public class InitialPersonInventory: Document
    {
        [BsonElement("legal_real_person_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string LegalRealPersonId { get; set; }
        [BsonElement("initial_inventory_core_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string InitialInventoryCoreId { get; set; }
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