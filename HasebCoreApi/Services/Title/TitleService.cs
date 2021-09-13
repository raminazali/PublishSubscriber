using HasebCoreApi.Models;
using System.Threading.Tasks;
using HasebCoreApi.Helpers;
using System.Collections.Generic;
using MongoDB.Bson;
using System;
using System.Linq;

namespace HasebCoreApi
{
    public class TitleService : ITitleService
    {
        private readonly IMongoRepository<Title> _titleRepo;

        public TitleService(IMongoRepository<Title> titleRepo)
        {
            _titleRepo = titleRepo;
        }

        public async Task<List<Title>> Get()
        {
            return await _titleRepo.FindAll();
        }

        public async Task<Title> Get(string id)
        {
            return await _titleRepo.FindByIdAsync(id);
        }

        public async Task<Title> Exists(string name)
        {
            return await _titleRepo.FindOneAsync(x => x.Name == name);
        }

        public async Task<Title> Create(Title title)
        {
            var _name = title.Name.Trim();
            var dup = await _titleRepo.FindOneAsync(x => x.Name == _name);
            if (dup != null)
            {
                throw new TitleDuplicateException { Title = dup };
            }

            var _title = new Title { Name = _name };
            await _titleRepo.InsertOneAsync(_title);
            return _title;
        }

        public async Task<Title> Update(Title title)
        {
            var _name = title.Name.Trim();
            var dup = await _titleRepo.FindOneAsync(x => x.Name == _name && x.Id != title.Id);
            if (dup != null)
            {
                throw new TitleDuplicateException { Title = dup };
            }

            await _titleRepo.ReplaceOneAsync(title);
            return title;
        }

    }

    /// <summary>
    /// Title is duplicated
    /// </summary>
    public class TitleDuplicateException : Exception { public Title Title { get; set; } }
}






