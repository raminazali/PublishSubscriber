using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;
using HasebCoreApi.Helpers;
using HasebCoreApi.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HasebCoreApi.Services.Commodities
{
    public class CommodityService : ICommodityService
    {
        private readonly IMongoRepository<Commodity> _commodity;
        private readonly IMongoRepository<CommodityGroup> _commodityGroup;
        private readonly IMongoRepository<Branch> _branch;
        private readonly IMongoRepository<Unit> _unit;
        private readonly IMongoRepository<Period> _periode;
        private readonly FTPServer _ftp;

        public CommodityService(FTPServer ftp, IMongoRepository<Commodity> commodity,
            IMongoRepository<CommodityGroup> commodityGroup,
            IMongoRepository<Branch> branch,
            IMongoRepository<Unit> unit,
            IMongoRepository<Period> periode
            )
        {
            _ftp = ftp;
            _commodityGroup = commodityGroup;
            _commodity = commodity;
            _branch = branch;
            _unit = unit;
            _periode = periode;
        }
        public async Task<Commodity> Create(Commodity commodity, List<IFormFile> files)
        {
            var branch = _branch.FindById(commodity.BranchId);
            if (branch == null)
            {
                throw new BranchNotFoundException();
            }

            if (!string.IsNullOrWhiteSpace(commodity.CommodityGroupId))
            {
                var commodityGroup = _commodityGroup.FindById(commodity.CommodityGroupId);
                if (commodityGroup == null)
                {
                    throw new CommodityGroupNotFoundException();
                }
            }

            var unitMain = _unit.FindById(commodity.UnitMainId);
            if (unitMain == null)
            {
                throw new UnitMainNotFoundException();
            }

            if (!string.IsNullOrWhiteSpace(commodity.UnitSubId))
            {
                var unitSub = _unit.FindById(commodity.UnitSubId);
                if (unitSub == null)
                {
                    throw new UnitSubNotFoundException();
                }
            }
            //---------------------------------------------------------------------------------
            var fileInserted = files.InsertFileToFtp(_ftp, "/Images/Commodities/");
            if (fileInserted != null)
            {
                commodity.Photo = fileInserted;
                await _commodity.InsertOneAsync(commodity);
                return commodity;
            }
            //---------------------------------------------------------------------------------
            await _commodity.InsertOneAsync(commodity);
            return commodity;
        }

        public IQueryable<object> GetAll(string branchId)
        {
            var branch = _branch.FindOneAsync(x => x.Id == branchId);
            if (branch == null) throw new BranchNotFoundException();
            var query = from a in _commodity.AsQueryable().Where(x => x.BranchId == branchId)
                        join b in _unit.AsQueryable() on a.UnitMainId equals b.Id
                        select new
                        {
                            a.Id,
                            a.NameOne,
                            a.CommodityCode,
                            a.IsActive,
                            UnitName = b.NameOne
                        };
            return query;
        }

        public async Task<object> Get(string id)
        {
            var comm = await _commodity.FindByIdAsync(id);
            if (comm == null) throw new CommodityNotFoundException();

            var lists = new List<object> { };
            var commodityGpId = comm.CommodityGroupId;
            if (commodityGpId != null)
            {
                bool subToId = true;
                while (subToId != false)
                {
                    var data = await _commodityGroup.FindOneAsync(x => x.Id == commodityGpId);
                    commodityGpId = data.SubToId;
                    if (data.SubToId == null)
                    {
                        subToId = false;
                    }
                    lists.Insert(0, data);
                }
            }

            return new
            {
                comm.AvgInventory,
                comm.BranchId,
                comm.Barcode,
                comm.Commission,
                comm.CommodityCode,
                comm.CommodityGroupId,
                comm.CreateDate,
                comm.Description,
                comm.Discounts,
                comm.Id,
                comm.InitialInventory,
                comm.IsActive,
                comm.IsCommodity,
                comm.IsPurchaseExempt,
                comm.IsSaleExempt,
                comm.MaximumInventory,
                comm.MinimumInventory,
                comm.NameOne,
                comm.NameTwo,
                comm.Photo,
                comm.Prices,
                comm.PurchaseDescription,
                comm.SaleDescription,
                comm.SerialNumber,
                comm.ServiceStuffId,
                comm.TechnicalCode,
                comm.TemplateCode,
                comm.TemplateName,
                comm.UnitMainId,
                comm.UnitMainValue,
                comm.UnitSubId,
                comm.UnitSubValue,
                comm.UpdateDate,
                comm.UpdaterId,
                comm.UserId,
                CommodityGroups = lists
            };
        }

        public IQueryable<object> Get(string branchId, bool? IsCommodity)
        {
            var branch = _branch.FindByIdAsync(branchId);
            if (branch == null) throw new BranchNotFoundException();
            var query = from a in _commodity.AsQueryable().Where(x => x.IsCommodity.Equals(IsCommodity) && x.BranchId == branchId)
                        join b in _unit.AsQueryable() on a.UnitMainId equals b.Id
                        select new
                        {
                            a.Id,
                            a.NameOne,
                            a.CommodityCode,
                            a.IsActive,
                            UnitName = b.NameOne
                        };
            return query;
        }

        public async Task<List<Commodity>> GetbyCommo(bool IsCommodity, string commodityGroupId)
        {
            return await _commodity.AsQueryable().Where(x => x.IsCommodity.Equals(IsCommodity) && x.CommodityGroupId.Equals(commodityGroupId)).ToListAsyncSafe();
        }

        public async Task<Commodity> Update(Commodity commodity, List<IFormFile> files)
        {
            var fileInserted = files.InsertFileToFtp(_ftp, "/Images/Commodities/");
            if (fileInserted != null)
            {
                commodity.Photo.AddRange(fileInserted);
            }
            await _commodity.ReplaceOneAsync(commodity);
            return commodity;
        }

        public async Task<Commodity> GetById(string id)
        {
            return await _commodity.FindByIdAsync(id);
        }

        // public IEnumerable<object> GetDiscountByPeriod(string branchId, string periodId, bool isCommodity)
        // {
            //var query = (from a in _commodity.AsQueryable()
            //             where a.IsCommodity.Equals(isCommodity) && a.BranchId == branchId
            //             from b in a.Discounts 
            //             select new
            //             {
            //                 b,
            //                 a.CommodityCode,
            //                 a.SerialNumber,
            //                 a.NameOne,
            //                 a.NameTwo,
            //                 a.TechnicalCode,
            //                 a.Barcode
            //             }).Where(x => x.b.PeriodId == periodId).Select(x => new
            //             {
            //                 x.Barcode,
            //                 x.CommodityCode,
            //                 x.NameOne,
            //                 x.NameTwo,
            //                 x.TechnicalCode,
            //                 x.SerialNumber,
            //                x.b.Percent
            //             });
            //var query2 = from c in _commodity.AsQueryable().SelectMany(x => x.Discounts).ToList();
            //var query2 = _commodity.AsQueryable().SelectMany(x => x.Discounts, (x, discount) => new
            //{
            //    x.NameOne,
            //    x.NameTwo,
            //    x.SerialNumber,
            //    x.TechnicalCode,
            //    x.Barcode,
            //    x.CommodityCode,
            //    discount
            //})
            //    //.Where(x => x.discount.PeriodId != periodId)
            //    .ToList();

            //var query = (from a in _commodity.AsQueryable().Where(a => a.IsCommodity == isCommodity && a.BranchId == branchId 
            //             && (a.Discounts.Count > 0 && a.Discounts.Any(x => x.PeriodId == periodId) || a.Discounts.Count == 0))
            //             //from b in a.Discounts
            //                 //where (a.Discounts.Count > 0 && a.Discounts.Any(x =>x.PeriodId == periodId) || a.Discounts.Count == 0 )
            //             select new
            //             {
            //                 Discount = a.Discounts,
            //                 a.NameOne,
            //                 a.NameTwo,
            //                 a.SerialNumber,
            //                 a.TechnicalCode,
            //                 a.Barcode,
            //                 a.CommodityCode
            //             });
            //.Where(x =>  x.Discount.PeriodId == periodId);
            //.Select(x => new { x.Discount.Percent, x.NameOne, x.NameTwo, x.SerialNumber, x.TechnicalCode, x.Barcode, x.CommodityCode });

            //var query = (from a in _commodity.AsQueryable().Where(a => a.IsCommodity == isCommodity && a.BranchId == branchId)
            //             select new
            //             {
            //                 a.Discounts,
            //                 a.NameOne,
            //                 a.NameTwo,
            //                 a.SerialNumber,
            //                 a.TechnicalCode,
            //                 a.Barcode,
            //                 a.CommodityCode,
            //                 a.Id
            //             }).ToList();
            //.Select(x => new { x.Discount.Percent, x.NameOne, x.NameTwo, x.SerialNumber, x.TechnicalCode, x.Barcode, x.CommodityCode });
            //var res = from a in query
            //          select new
            //          {
            //              Discount = a.Discounts.Where(x =>x.PeriodId == periodId),
            //              a.NameOne,
            //              a.NameTwo,
            //              a.SerialNumber,
            //              a.TechnicalCode,
            //              a.Barcode,
            //              a.CommodityCode,
            //              a.Id
            //          };



            //var q1 = from a in _commodity.AsQueryable().Where(x =>x.Discounts.Any(x =>x.PeriodId == periodId))
            //         //from b in a.Discounts
            //         select  a;

            //var list = q1.ToList().Select(x => x.Id);

            //var q2 = from a in _commodity.AsQueryable() .Where(x => !list.Contains(x.Id))
            //         //from b in a.Discounts
            //         select  a;
            //    ommodity_service.aggregate([
            //    { '$match': { 'is_commodity': true,  'branch_id': ObjectId("5fb8fb15c13175402383ff66"),'discount.period_id':ObjectId("5fc5fbf2bf4844154f3aba12")} },
            //    { '$unwind': '$discount'},
            //    { '$match': { 'discount.period_id':ObjectId("5fc5fbf2bf4844154f3aba12")} },
            //    { '$project': { '_id':0,'commodity_code':1,'name_one':1,'name_two':1,'technical_code':1,'serial_number':1,'barcode':1,'discount.persent':1 } },
            //    { '$sort': { 'discount.persent':-1 } },
            //    { $unionWith:
            //        {
            //        coll: "commodity_service", pipeline:[ { '$match': { 'is_commodity': true,  'branch_id': ObjectId("5fb8fb15c13175402383ff66"),'discount.period_id':{$ne: ObjectId("5fc5fbf2bf4844154f3aba12")} } } ,
            //    {
            //                '$project': {
            //                    '_id':0,'commodity_code':1,'name_one':1,'name_two':1,'technical_code':1,'serial_number':1,'barcode':1,
            //    'discount':{$cond: { if:{ $eq:[ObjectId("5fc5fbf2bf4844154f3aba12"), "$discount.period_id" ] },then: 0,else: 0} }
            //                }
            //            } ]}
            //    }



            //])


            // var match5 = new BsonDocument { { "$match", new BsonDocument { { "is_commodity", true }, { "branch_id", ObjectId.Parse(branchId) }, { "discount.period_id", ObjectId.Parse(periodId) } } } };
            // var project2 = new BsonDocument { { "$project", new BsonDocument { { "commodity_code", 1 }, { "name_one", 1 }, { "technical_code", 1 }, { "serial_number", 1 }, { "barcode", 1 },
            //     { "discount",1 } } } };
            // //---------------------------------------------------------------------------------------------------------------------------
            //
            // // var unionwith = new BsonDocument { { "coll", "commodity_service" }, new BsonDocument[] {} };
            //
            // var pipeline = new[] { match5, project2 };
            // //---------------------------------------------------------------------------------------------------------------------------
            //
            // var match1 = new BsonDocument { { "$match", new BsonDocument { { "is_commodity", true }, { "branch_id", ObjectId.Parse(branchId) }, { "discount.period_id", ObjectId.Parse(periodId) } } } };
            // var unwind = new BsonDocument { { "$unwind", "$discount" } };
            // var match2 = new BsonDocument { { "$match", new BsonDocument { { "discount.period_id", ObjectId.Parse(periodId) } } } };
            // var project = new BsonDocument { { "$project", new BsonDocument { { "commodity_code", 1 }, { "name_one", 1 }, { "technical_code", 1 }, { "serial_number", 1 }, { "barcode", 1 }, { "discount.persent", 1 } } } };
            // var sort = new BsonDocument { { "$sort", new BsonDocument { { "discount.persent", -1 } } } };
            // //---------------------------------------------------------------------------------------------------------------------------
            //
            // var pipeline2 = new[] { match1, unwind, match2, project, sort };
            //
            // //var query = _commodity.GetMongoCollection().Aggregate<object>(pipeline).ToEnumerable();
            //
            // var query2 = _commodity.GetMongoCollection().Aggregate<object>(pipeline2); //.ToEnumerable();
            // return query2.To;
            //_commodity.AsQueryable().Aggregate([
            //    { $sort: '_id' , 1}
            //    ])
            //var q1 = _commodity.AsQueryable().SelectMany(x => x.Discounts, (x, discount) => new
            //{
            //    x.Id,
            //    x.NameOne,
            //    x.NameTwo,
            //    x.SerialNumber,
            //    x.TechnicalCode,                      fix me
            //    x.Barcode,
            //    x.CommodityCode,
            //    Percent = discount.Percent.ToString(),
            //    discount.PeriodId
            //}).Where(x => x.PeriodId == periodId);

            //var list = q1.ToList().Select(x => x.Id);

            //var q2 = _commodity.AsQueryable().SelectMany(x => x.Discounts, (x, discount) => new
            //{
            //    x.Id,
            //    x.NameOne,
            //    x.NameTwo,
            //    x.SerialNumber,
            //    x.TechnicalCode,
            //    x.Barcode,
            //    x.CommodityCode,
            //    discount
            //}).Where(x => !list.Contains(x.Id));

            //var q2 = from a in _commodity.AsQueryable().Where(x => !list.Contains(x.Id))
            //         select new
            //         {
            //             a.Id,
            //             a.NameOne,
            //             a.NameTwo,
            //             a.SerialNumber,
            //             a.TechnicalCode,
            //             a.Barcode,                       fix me
            //             a.CommodityCode,
            //             Percent = "",
            //             PeriodId = "",
            //         };

            //var res = q1.AsEnumerable().Union(q2.AsEnumerable());

            //var s = res.ToString();

            //return res;

            //return q1.AsEnumerable().Union(q2.AsEnumerable());


            //return query.Concat(query2).Distinct();
        
        public async Task<string> DeleteFile(string fileNameAndPath)
        {
            if (fileNameAndPath == null) throw new FileNameOrPathNullException();

            try
            {
                var data = Helper.DeleteFile(_ftp, fileNameAndPath);

                if (data.Length != 0)
                {
                    var comm = await _commodity.FilterByAsync(x => x.Photo.Contains(fileNameAndPath));
                    foreach (var item in comm)
                    {
                        var t = item.Photo.Remove(fileNameAndPath);
                        if (t == true) await _commodity.ReplaceOneAsync(item);
                    }
                }
                return data;
            }
            catch
            {
                throw new FileNameOrPathNullException();
            }
        }

        public async Task Delete(string id)
        {
            string[] keys = id.Split(",");

            foreach (var item in keys)
            {

                if (string.IsNullOrWhiteSpace(item) || item.Length != 24) throw new IdLengthNotEqual();
                var commodity = await _commodity.FindByIdAsync(item);

                if (commodity.Discounts.Count != 0) throw new CommodityReferencedToCommodityException();

                if (commodity == null)
                {
                    throw new CommodityNotFoundException();
                }

                foreach (var item2 in commodity.Photo)
                {

                    var t = Helper.DeleteFile(_ftp, item2);

                    if (t.Length == 0) throw new ImageDeleteException();
                }


                await _commodity.DeleteByIdAsync(item);
            }
        }
    }
}


public class BranchNotFoundException : Exception { }
public class CommodityGroupNotFoundException : Exception { }
public class CommodityNotFoundException : Exception { }
public class UnitMainNotFoundException : Exception { }
public class UnitSubNotFoundException : Exception { }
public class ImageLengthSomuchException : Exception { }
public class NoFileFoundException : Exception { }
public class FileNameOrPathNullException : Exception { }
public class CommodityReferencedToCommodityException : Exception { }
public class ImageDeleteException : Exception { }