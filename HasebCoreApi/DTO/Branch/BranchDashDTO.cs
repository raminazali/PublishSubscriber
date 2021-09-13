using HasebCoreApi.DTO.Common;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HasebCoreApi.DTO.Branch
{
    /// <summary>
    /// For use 
    /// </summary>
    public class BranchDashDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public Owner Owner { get; set; }
        public DateTime StartDate { get; set; }
    }
}
