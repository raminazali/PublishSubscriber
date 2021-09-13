using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Mongo.Models
{
    [BsonCollection("Costumer")]
    public class ReqDataMongo: Document
    {
        [BsonElement("Path")]
        public string Path { get; set; }
        [BsonElement("Body")]
        public List<Customer> Body { get; set; }
        [BsonElement("Header")]
        public Dictionary<string, string> Header{ get; set; }
        [BsonElement("IP")]
        public string IP { get; set; }
    }
}
