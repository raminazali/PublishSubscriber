using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HasebCoreApi.Models
{
    [BsonCollection("city")]
    public partial class City : Document
    {
        [BsonElement("province_code")]
        [Required]
        public int ProvinceCode { get; set; }
        [BsonElement("name")]
        [StringLength(45, MinimumLength = 1)]
        [Required]
        public string Name { get; set; }
        [Required]
        [BsonElement("code")]
        public int Code { get; set; }
    }
}
