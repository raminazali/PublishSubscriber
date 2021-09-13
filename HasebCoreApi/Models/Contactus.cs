using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace HasebCoreApi.Models
{
    [BsonCollection("contact_us")]
    public class Contactus : Document
    {
        public Contactus()
        {
            Answer = new List<ContactusAnswer>();
        }

        [BsonElement("name")]
        [StringLength(70, MinimumLength = 1)]
        public string Name { get; set; }
        [BsonElement("email")]
        [StringLength(100, MinimumLength = 1)]
        public string Email { get; set; }
        [BsonElement("subject")]
        [StringLength(100, MinimumLength = 1)]
        public string Subject { get; set; }
        [BsonElement("text")]
        public string Text { get; set; }
        [BsonElement("is_answerd")]
        public bool IsAnswered { get; set; } = false;
        [BsonElement("answer")]
        public List<ContactusAnswer> Answer { get; set; } = new List<ContactusAnswer> { };
        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public class ContactusAnswer
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public ObjectId Id { get; set; }
            [BsonElement("text")]
            public string Text { get; set; }
            [BsonElement("user_id")]
            [BsonRepresentation(BsonType.ObjectId)]
            public string UserId { get; set; }
        }
    }
}
