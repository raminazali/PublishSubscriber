// using HasebCoreApi.Models;
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace HasebCoreApi.Services.DB
// {
//     public class DbService : IDbService
//     {
//         //private readonly IRepositoryWrapper _repositoryWrapper;
//         private readonly IMongoRepository<Db> _Db;
//         public DbService(AppSettings appSettings, IMongoRepository<Db> Db)
//         {
//             _Db = Db;
//         }

//         public async Task<Boolean> DeleteDb(string Id)
//         {

//            await _Db.DeleteByIdAsync(Id);
//            return true;
//         }
//         // public async Task<Db> GetListsByrole(string UserId)
//         // {
//         //    var data = await _Db.FindByIdAsync(UserId);
//         //    return data;
//         // }


//     }
// }
// public class DbNotFoundException : Exception { }