using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    public class RelatedAddress
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("is_main")]
        public bool IsMain { get; set; } = false;
        [StringLength(70, MinimumLength = 1)]
        [BsonElement("province")]
        [Required]
        public string Province { get; set; }
        [StringLength(70, MinimumLength = 1)]
        [BsonElement("city")]
        [Required]
        public string City { get; set; }
        [BsonElement("address")]
        [Required]
        public string Address { get; set; }
        [BsonElement("buyerpostalcode")]
        [StringLength(10, MinimumLength = 1)]
        public string BuyerPostalCode { get; set; }

    }
}
