using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace HasebCoreApi.Models
{
    public class RelatedPhone 
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("is_main")]
        public bool IsMain { get; set; } = false;
        [BsonElement("is_phone")]
        public bool IsPhone { get; set; } = true;
        [BsonElement("phone_number")]
        [StringLength(10, MinimumLength = 1)]
        [Required]
        public string PhoneNumber { get; set; }

    }
}
