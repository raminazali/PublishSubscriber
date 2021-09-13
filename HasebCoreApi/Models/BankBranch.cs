using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("bank_branch")]
    public class BankBranch : Document
    {
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("branch_id")]
        public string BranchId { get; set; }
        [Required]
        [BsonElement("bank_branch_name")]
        public string BankBranchName { get; set; }
        [Required]
        [BsonElement("bank_branch_code")]
        public string BankBranchCode { get; set; }
        [Required]
        [BsonElement("bank_name")]
        public string BankName { get; set; }
    }
}
