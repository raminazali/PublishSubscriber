using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HasebCoreApi.Models
{
    [BsonCollection("plan")]
    public partial class Plan : Document
    {
        [BsonElement("duration")]
        [Required(ErrorMessage = "req_duration")]
        [Range(1, 1000000, ErrorMessage = "req_duration_min_max")]
        public int Duration { get; set; }
        [BsonElement("name")]
        [StringLength(45, MinimumLength = 1)]
        [Required(ErrorMessage = "req_name")]
        public string Name { get; set; }
    }
}
