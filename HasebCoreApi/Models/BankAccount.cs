using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("bank_account")]
    public class BankAccount : Document
    {
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("branch_id")]
        public string BranchId { get; set; }
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("currency_id")]
        public string CurrencyId { get; set; }
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("bank_branch_id")]
        public string BankBranchId { get; set; }
        [Required]
        [BsonElement("code")]
        public string Code { get; set; }
        [Required]
        [BsonElement("account_type")]
        public string AccountType { get; set; }
        [BsonElement("card_number")]
        public string CardNumber { get; set; }
        [Required]
        [BsonElement("IBAN")]
        public string IBAN { get; set; }
        [BsonElement("account_name_one")]
        [Required]
        public string AccountNameOne { get; set; }
        [BsonElement("account_name_two")]
        public string AccountNameTwo { get; set; }
        [BsonElement("is_active")]
        public bool IsActive { get; set; } = true;
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("user_id")]
        public string UserId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("updater_id")]
        public string UpdaterId { get; set; }
        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [BsonElement("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}
