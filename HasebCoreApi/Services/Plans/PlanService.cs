using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Helpers;
using HasebCoreApi.Helpers.Common;
using HasebCoreApi.Models;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;

namespace HasebCoreApi.Services.Plans
{
    public class PlanService : IPlanService
    {
        private readonly IMongoRepository<Plan> _plan;
        public PlanService(IMongoRepository<Plan> plan)
        {
            _plan = plan;
        }

        public async Task Create(Plan plan)
        {
            await _plan.InsertOneAsync(plan);
        }

        public async Task<List<Plan>> Get()
        {
            return await _plan.AsQueryable().ToListAsyncSafe();
        }
        public async Task<Plan> Get(string id)
        {
            var data = await _plan.FindByIdAsync(id);
            return data;
        }
        public async Task Update(Plan plan)
        {
            await _plan.ReplaceOneAsync(plan);
        }
    }
}
/// <summary>
/// No Plan Found In DataBase
/// </summary>
public class PlanNotFoundException : Exception { }