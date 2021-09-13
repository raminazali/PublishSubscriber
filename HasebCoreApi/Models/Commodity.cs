using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HasebCoreApi.Models
{
    [BsonCollection("commodity_service")]
    public class Commodity : Document
    {
        public Commodity()
        {
            Prices = new List<Price>();
            Discounts = new List<Discount>();
            Commission = new List<Commission>();
        }

        [BsonElement("branch_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string BranchId { get; set; }

        [BsonElement("commodity_group_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommodityGroupId { get; set; }

        [BsonElement("unit_sub_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UnitSubId { get; set; }

        [BsonElement("unit_main_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string UnitMainId { get; set; }

        [BsonElement("price")]
        public List<Price> Prices { get; set; } = new List<Price> { };
        [BsonElement("discount")]
        public List<Discount> Discounts { get; set; } = new List<Discount> { };
        [BsonElement("commission")]
        public List<Commission> Commission { get; set; } = new List<Commission> { };

        [StringLength(15, MinimumLength = 1)]
        [BsonElement("commodity_code")]
        [Required]
        public string CommodityCode { get; set; }

        [BsonElement("name_one")]
        [StringLength(70, MinimumLength = 1)]
        [Required]
        public string NameOne { get; set; }

        [StringLength(15, MinimumLength = 1)]
        [BsonElement("technical_code")]
        public string TechnicalCode { get; set; }

        [StringLength(70, MinimumLength = 1)]
        [BsonElement("name_two")]
        public string NameTwo { get; set; }

        [StringLength(15, MinimumLength = 1)]
        [BsonElement("serial_number")]
        public string SerialNumber { get; set; }

        [StringLength(15, MinimumLength = 1)]
        [BsonElement("barcode")]
        public string Barcode { get; set; }

        [StringLength(17, MinimumLength = 1)]
        [BsonElement("service_stuff_id")]
        [Required]
        public string ServiceStuffId { get; set; }

        [BsonElement("template_code")]
        public int? TemplateCode { get; set; }

        [StringLength(45, MinimumLength = 1)]
        [BsonElement("template_name")]
        public string TemplateName { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("unit_main_value")]
        public int? UnitMainValue { get; set; }

        [BsonElement("unit_sub_value")]
        public int? UnitSubValue { get; set; }

        [BsonElement("photo")]
        public List<string> Photo { get; set; } = new List<string> { };

        [BsonElement("minimum_inventory")]
        public long? MinimumInventory { get; set; }

        [BsonElement("maximum_inventory")]
        public long? MaximumInventory { get; set; }

        [BsonElement("initial_inventory")]
        public List<YearEmbedd> InitialInventory { get; set; } = new List<YearEmbedd> { };

        [BsonElement("avg_inventory")]
        public List<YearEmbedd> AvgInventory { get; set; } = new List<YearEmbedd> { };

        [BsonElement("purchase_description")]
        public string PurchaseDescription { get; set; }

        [BsonElement("sale_description")]
        public string SaleDescription { get; set; }

        [BsonElement("is_active")]
        public bool IsActive { get; set; } = true;
        [BsonElement("is_commodity")]
        public bool IsCommodity { get; set; } = true;

        [BsonElement("is_purchase_exempt")]
        public bool IsPurchaseExempt { get; set; } = false;

        [BsonElement("is_sale_exempt")]
        public bool IsSaleExempt { get; set; } = false;

        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string UserId { get; set; }

        [BsonElement("updater_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UpdaterId { get; set; }

        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [BsonElement("update_date")]
        public DateTime? UpdateDate { get; set; }
    }

    public class Price
    {
        [BsonElement("amount")]
        [Required]
        public long Amount { get; set; }
        [BsonElement("period_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string PeriodId { get; set; }
        [BsonElement("year")]
        [Required]
        public int Year { get; set; }
    }

    public class Discount
    {
        [BsonElement("persent")]
        //[Required]
        public int Percent { get; set; }
        [BsonElement("period_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string PeriodId { get; set; }
        [BsonElement("year")]
        [Required]
        public int Year { get; set; }
    }
    public class Commission
    {
        [BsonElement("persent")]
        [Required]
        public int Percent { get; set; }
        [BsonElement("period_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string PeriodId { get; set; }
        [BsonElement("year")]
        [Required]
        public int? Year { get; set; }
        [BsonElement("code")]
        [Required]
        public string Code { get; set; }
        [BsonElement("name_one")]
        [Required]
        public string NameOne { get; set; }
        [BsonElement("name_two")]
        public string NameTwo { get; set; }
        [BsonElement("is_Active")]
        public bool IsActive { get; set; } = true;
    }
    public class YearEmbedd
    {
        [BsonElement("year")]
        [Required]
        public int Year { get; set; }
        [BsonElement("amount")]
        [Required]
        public long Amount { get; set; }

    }
}