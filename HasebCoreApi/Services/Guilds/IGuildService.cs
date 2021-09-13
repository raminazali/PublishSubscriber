using System.Collections.Generic;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi.Services.Guilds
{
    public interface IGuildService
    {
        /// <summary>
        /// Get All Of Guilds List
        /// </summary>
        /// <exception cref="GuildListEmptyException">throw when No Guild In Database (guild Collection Empty) </exception>
        /// <returns></returns>
        Task<List<Guild>> Get();
        /// <summary>
        /// Get List of Guilds That have this specific Id
        /// </summary>
        /// <exception cref="NoGuildFoundException">throw when Not Found Specific Guild That we want with Id </exception>
        /// <returns></returns>
        Task<Guild> Get(string id);
        /// <summary>
        /// You Can Create New Guild With This Method
        /// </summary>
        /// <returns></returns>
        Task Create(Guild guild);
        /// <summary>
        /// You Can Update Your Guild With This Update Method
        /// </summary>
        /// <returns></returns>
        Task Update(Guild guild);
    }
}