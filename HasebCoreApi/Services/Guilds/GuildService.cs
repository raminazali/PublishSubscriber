using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.Guilds
{
    public class GuildService : IGuildService
    {
        private readonly IMongoRepository<Guild> _Guild;

        public GuildService(IMongoRepository<Guild> Guild)
        {
            _Guild = Guild;
        }
        public async Task Create(Guild guild)
        {
            await _Guild.InsertOneAsync(guild);
        }

        public async Task<List<Guild>> Get()
        {
            var data = await _Guild.FindAll();
            if(data == null)throw new GuildListEmptyException();
            return data;
        }

        public async Task<Guild> Get(string id)
        {
            var data = await _Guild.FindByIdAsync(id);
            if (data == null) throw new NoGuildFoundException();
            return data;
        }

        public async Task Update(Guild guild)
        {
            await _Guild.ReplaceOneAsync(guild);
        }
    }
}
/// <summary>
/// No Guild Found
/// </summary>
public class NoGuildFoundException : Exception { }
/// <summary>
/// Nothing In Guild Database For Adding to the List
/// </summary>
public class GuildListEmptyException : Exception { }