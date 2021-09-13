using HasebCoreApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using HasebCoreApi.Helpers;

namespace HasebCoreApi.Services.CommodityGroups
{
    public class CommodityGroupService : ICommodityGroupService
    {
        private readonly IMongoRepository<CommodityGroup> _commodityGroup;
        private readonly IMongoRepository<Commodity> _commodity;
        public CommodityGroupService(IMongoRepository<CommodityGroup> commodityGroup, IMongoRepository<Commodity> commodity)
        {
            _commodityGroup = commodityGroup;
            _commodity = commodity;
        }

        public async Task Create(CommodityGroup commodityGroup)
        {
            if (!string.IsNullOrWhiteSpace(commodityGroup.SubToId))
            {
                var sub = await _commodityGroup.FindByIdAsync(commodityGroup.SubToId);
                if (sub == null)
                {
                    throw new CommodityGroupNotFoundException();
                }
            }
            await _commodityGroup.InsertOneAsync(commodityGroup);
        }

        public async Task Delete(string id)
        {
            var subs = await _commodityGroup.FindOneAsync(x=>x.SubToId == id);
            if (subs != null) throw new CommodityGroupReferencedToItselfException();

            var commo = await _commodity.FindOneAsync(x => x.CommodityGroupId == id);
            if (subs != null) throw new CommodityGroupReferencedToCommodityException { Commodity = commo };

            await _commodityGroup.DeleteByIdAsync(id);
        }

        public async Task<List<CommodityGroup>> Get()
        {
            return await _commodityGroup.FindAll();
        }

        public async Task<CommodityGroup> Get(string id)
        {
            return await _commodityGroup.FindByIdAsync(id);
        }

        public async Task<List<CommodityGroup>> GetByParent(string parentId, string branchId)
        {
            string _parentId = null;
            if (!string.IsNullOrWhiteSpace(parentId) && parentId.Length == 24)
            {
                _parentId = parentId;
            }
            return await _commodityGroup.AsQueryable().Where(x => x.SubToId == _parentId && x.BranchId == branchId).ToListAsyncSafe();
        }

        public async Task Update(CommodityGroup commodityGroup)
        {
            if (!string.IsNullOrWhiteSpace(commodityGroup.SubToId))
            {
                var sub = await _commodityGroup.FindByIdAsync(commodityGroup.SubToId);
                if (sub == null)
                {
                    throw new CommodityGroupNotFoundException();
                }
            }

            await _commodityGroup.ReplaceOneAsync(commodityGroup);
        }
    }
    public class CommodityGroupNotFoundException : Exception { }

}

public class CommodityGroupReferencedToItselfException : Exception { }
public class CommodityGroupReferencedToCommodityException : Exception { public Commodity Commodity { get; set; } }