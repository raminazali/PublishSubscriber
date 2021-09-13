using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    public class RelatedPerson
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("first_name")]
        [StringLength(70, MinimumLength = 1)]
        [Required]
        public string FirstName { get; set; }
        [BsonElement("last_name")]
        [StringLength(70, MinimumLength = 1)]
        [Required]
        public string LastName { get; set; }
        [BsonElement("post")]
        [StringLength(70, MinimumLength = 1)]
        [Required]
        public string Post { get; set; }
        [BsonElement("phone")]
        [StringLength(10, MinimumLength = 1)]
        [Required]
        public string Phone { get; set; }
        [BsonElement("address")]
        [Required]
        public string Address { get; set; }
    }
}
