using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HasebCoreApi.Models
{
    [BsonCollection("module")]

    public partial class Module : Document
    {
        [BsonElement("name")]
        [StringLength(100, MinimumLength = 1)]
        [Required]
        public string Name { get; set; }
    }
}
