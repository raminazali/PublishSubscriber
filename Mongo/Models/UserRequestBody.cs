using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mongo.Models
{
    [BsonCollection("User")]
    public class UserRequestBody: Document
    {
        [BsonElement("Path")]
        public string Path { get; set; }
        [BsonElement("Body")]
        public List<User> Body { get; set; }
        [BsonElement("Header")]
        public Dictionary<string, string> Header { get; set; }
        [BsonElement("IP")]
        public string IP { get; set; }
    }
}
