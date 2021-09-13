using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HasebCoreApi.Models
{
    [BsonCollection("province")]
    public partial class Province : Document
    {
        [BsonElement("name")]
        [StringLength(45, MinimumLength = 1)]
        [Required]
        public string Name { get; set; }
        [Required]
        [BsonElement("code")]
        public int Code { get; set; }
    }
}
