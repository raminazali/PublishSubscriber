using HasebCoreApi.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HasebCoreApi.Helpers;
using MongoDB.Driver.Linq;
using Microsoft.EntityFrameworkCore;
using MD.PersianDateTime;
using System.Globalization;

namespace HasebCoreApi.Services.Periods
{
    public class PeriodService : IPeriodService
    {
        private readonly IMongoRepository<Period> _period;
        private readonly IMongoRepository<Commodity> _commodity;
        private readonly IMongoRepository<Branch> _branch;
        public PeriodService(IMongoRepository<Period> pricePeriod, IMongoRepository<Commodity> commodity, IMongoRepository<Branch> branch)
        {
            _period = pricePeriod;
            _commodity = commodity;
            _branch = branch;
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------  Price  ======= 1
        public async Task<Period> Create(Period period)
        {
            foreach (var commodityAmount in period.CommodityAmounts)
            {
                var commodity = await _commodity.FindByIdAsync(commodityAmount.CommodityId);
                if (commodity == null)
                {
                    throw new CommodityNotFoundException();
                }

                string periodId = "";

                var pricePeriod_old = await _period.FindOneAsync(x => x.FromDate == period.FromDate && x.ToDate == period.ToDate && x.BranchId == period.BranchId && x.Type == period.Type && x.Reference == 1);
                //---------------------------------------------------------------------------------------------
                if (pricePeriod_old == null)
                {

                    var pricePeriod_overlap = await this._period.FindOneAsync(x => ((x.FromDate <= period.FromDate && x.ToDate >= period.FromDate && x.Type == period.Type && x.Reference == 1) || (x.FromDate <= period.ToDate && x.ToDate >= period.ToDate && x.Type == period.Type && x.Reference == 1)) && x.BranchId == period.BranchId);
                    if (pricePeriod_overlap != null)
                    {
                        throw new PricePeriodOverlapException { PricePeriod = pricePeriod_overlap };
                    }
                    var __period = new Period
                    {
                        BranchId = period.BranchId,
                        FromDate = period.FromDate.Date,
                        ToDate = period.ToDate.Date,
                        Type = period.Type,
                        UserId = period.UserId,
                        Isbuy = period.Isbuy,
                        Reference = 1
                    };
                    await this._period.InsertOneAsync(__period);
                    periodId = __period.Id;
                }
                else
                {
                    periodId = pricePeriod_old.Id;
                }
                //---------------------------------------------------------------------------------------------
                if (commodity.Prices.Any(x => x.PeriodId == periodId))
                {
                    throw new CommodityPricePeriodDuplicateException();
                }
                // This Part Is Embeded to Commodity Table
                var price = new Price { Amount = commodityAmount.Amount, PeriodId = periodId, Year = commodityAmount.Year };

                commodity.Prices.Add(price);

                await _commodity.ReplaceOneAsync(commodity);
            }
            return period;
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------  Discount  ======= 2
        public async Task<Period> CreateDiscount(Period discountPeriod)
        {
            foreach (var Discount in discountPeriod.PeriodDiscount)
            {
                var commodity = await _commodity.FindByIdAsync(Discount.CommodityId);
                if (commodity == null)
                {
                    throw new CommodityNotFoundException();
                }

                string periodId = "";

                var discountPeriod_old = await _period.FindOneAsync(x => x.FromDate == discountPeriod.FromDate && x.ToDate == discountPeriod.ToDate && x.BranchId == discountPeriod.BranchId && x.Type == discountPeriod.Type && x.Reference == 2);
                //---------------------------------------------------------------------------------------------
                if (discountPeriod_old == null)
                {
                    var discountPeriod_overlap = await _period.FindOneAsync(x => ((x.FromDate <= discountPeriod.FromDate && x.ToDate >= discountPeriod.FromDate && x.Type == discountPeriod.Type && x.Reference == 2) || (x.FromDate <= discountPeriod.ToDate && x.ToDate >= discountPeriod.ToDate && x.Type == discountPeriod.Type && x.Reference == 2)) && x.BranchId == discountPeriod.BranchId);
                    if (discountPeriod_overlap != null)
                    {
                        throw new PricePeriodOverlapException { PricePeriod = discountPeriod_overlap };
                    }
                    var period = new Period
                    {
                        BranchId = discountPeriod.BranchId,
                        FromDate = discountPeriod.FromDate.Date,
                        ToDate = discountPeriod.ToDate.Date,
                        Type = discountPeriod.Type,
                        UserId = discountPeriod.UserId,
                        Isbuy = discountPeriod.Isbuy,
                        Reference = 2
                    };
                    await _period.InsertOneAsync(period);
                    periodId = period.Id;
                }
                else
                {
                    periodId = discountPeriod_old.Id;
                }
                //---------------------------------------------------------------------------------------------
                if (commodity.Discounts.Any(x => x.PeriodId == periodId))
                {
                    throw new CommodityPricePeriodDuplicateException();
                }
                // This Part Is Embeded to Commodity Table

                var discount = new Discount { Percent = Discount.Percent, PeriodId = periodId, Year = Discount.Year };

                commodity.Discounts.Add(discount);

                await _commodity.ReplaceOneAsync(commodity);
            }
            return discountPeriod;
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------  Commission ======= 3

        public async Task<Period> Createcommission(Period commissionPeriod)
        {
            foreach (var commission in commissionPeriod.PeriodCommission)
            {
                var commodity = await _commodity.FindByIdAsync(commission.CommodityId);
                if (commodity == null)
                {
                    throw new CommodityNotFoundException();
                }

                string periodId = "";

                var pricePeriod_old = await _period.FindOneAsync(x => x.FromDate == commissionPeriod.FromDate && x.ToDate == commissionPeriod.ToDate && x.BranchId == commissionPeriod.BranchId && x.Type == commissionPeriod.Type && x.Reference == 3);
                //---------------------------------------------------------------------------------------------
                if (pricePeriod_old == null)
                {
                    var pricePeriod_overlap = await _period.FindOneAsync(x => ((x.FromDate <= commissionPeriod.FromDate && x.ToDate >= commissionPeriod.FromDate && x.Type == commissionPeriod.Type && x.Reference == 3) || (x.FromDate <= commissionPeriod.ToDate && x.ToDate >= commissionPeriod.ToDate && x.Type == commissionPeriod.Type && x.Reference == 3)) && x.BranchId == commissionPeriod.BranchId);
                    if (pricePeriod_overlap != null)
                    {
                        throw new PricePeriodOverlapException { PricePeriod = pricePeriod_overlap };
                    }
                    var period = new Period
                    {
                        BranchId = commissionPeriod.BranchId,
                        FromDate = commissionPeriod.FromDate.Date,
                        ToDate = commissionPeriod.ToDate.Date,
                        Type = commissionPeriod.Type.ToLower(),
                        UserId = commissionPeriod.UserId,
                        Reference = 3
                    };
                    await _period.InsertOneAsync(period);
                    periodId = period.Id;
                }
                else
                {
                    periodId = pricePeriod_old.Id;
                }
                //---------------------------------------------------------------------------------------------
                if (commodity.Commission.Any(x => x.PeriodId == periodId))
                {
                    throw new CommodityPricePeriodDuplicateException();
                }

                var commi = new Commission
                {
                    Percent = commission.Percent,
                    NameOne = commission.NameOne,
                    Code = commission.Code,
                    IsActive = commission.IsActive,
                    NameTwo = commission.NameTwo,
                    Year = commission.Year,
                    PeriodId = periodId
                };
                // This Part Is Embeded to Commodity Table

                commodity.Commission.Add(commi);

                await _commodity.ReplaceOneAsync(commodity);
            }
            return commissionPeriod;
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------- 
        public async Task<object> GetCommodityPricePeriods(string branchId, string commodityId)
        {
            var result = from a in _commodity.AsQueryable().Where(x => x.BranchId == branchId && x.Id == commodityId).SelectMany(x => x.Prices)
                         join b in _period.AsQueryable()
                         on a.PeriodId equals b.Id
                         select new
                         {
                             a.Amount,
                             a.Year,
                             b.CreateDate,
                             b.FromDate,
                             b.Id,
                             b.ToDate,
                             b.Type,
                         };

            return await result.ToListAsyncSafe();
        }

        public async Task<object> GetByType(string branchId, string type, int reference , bool Isbuy)
        {
            var branch = await _branch.FindByIdAsync(branchId);
            if (branch == null) throw new BranchNotFoundException();

            var data = _period.AsQueryable().Where(x => x.BranchId == branchId && x.Type == type && x.Reference == reference && x.Isbuy == Isbuy).ToList();
            if (data.Count == 0) return null;

            Period data2 = data.Last();
            DateTime d = DateTime.Parse(data2.ToDate.AddDays(1).ToLocalTime().ToString());
            PersianCalendar pc = new PersianCalendar();

            return new
            {
                EndDate = string.Format("{0}/{1}/{2}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d))
            };
        }

        public IEnumerable<object> GetQueryable(string branchId, int reference, bool Isbuy)
        {
            var branch = _branch.FindByIdAsync(branchId);
            if (branch == null) throw new BranchNotFoundException();

            var query = _period.AsQueryable().Where(x => x.BranchId == branchId && x.Reference == reference && x.Isbuy == Isbuy).ToList();
            var data = new List<object> { };
            foreach (var item in query)
            {
                PersianCalendar pc = new PersianCalendar();
                DateTime d = DateTime.Parse(item.ToDate.ToLocalTime().ToString());
                DateTime d2 = DateTime.Parse(item.FromDate.ToLocalTime().ToString());

                var date1 = string.Format("{0}/{1}/{2}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d));
                var date2 = string.Format("{0}/{1}/{2}", pc.GetYear(d2), pc.GetMonth(d2), pc.GetDayOfMonth(d2));
                var forData = new { ToDate = date1, FromDate = date2, item.Id, item.Type };
                data.Add(forData);
            }
            return data;
        }
    }

    public class PricePeriodNotFoundException : Exception { };
    public class CommodityNotFoundException : Exception { };
    public class CommodityPricePeriodDuplicateException : Exception { };
    public class WrongRefrenceException : Exception { };
    public class PricePeriodOverlapException : Exception { public Period PricePeriod { get; set; } };
}
