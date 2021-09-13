using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HasebCoreApi.Models
{
    [BsonCollection("pos")]
    public class Pos : Document
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
        [BsonElement("bank_account_id")]
        public string BankAccountId { get; set; }
        [Required]
        [BsonElement("code")]
        public string Code { get; set; }
        [Required]
        [BsonElement("pos_name_one")]
        public string PosNameOne { get; set; }
        [BsonElement("pos_name_two")]
        public string PosNameTwo { get; set; }
        [BsonElement("device_connect")]
        public string DeviceConnect { get; set; }
        [Required]
        [BsonElement("terminal_number")]
        public string TerminalNumber { get; set; }
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
