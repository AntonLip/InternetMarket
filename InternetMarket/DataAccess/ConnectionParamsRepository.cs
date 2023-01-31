using System.Collections.Generic;
using System.Linq;
using Contributors.DataAccess;
using InternetMarket.Interfaces.Repository;
using InternetMarket.Models.DbModels;

namespace InternetMarket.DataAccess;

internal class ConnectionParamsRepository : BaseRepository<ConnectionParams>, IConnectionParamsRepository
{
    public ConnectionParamsRepository(AppDbContext context)
    :base(context)
    {
            
    }

    public ConnectionParams GetByHash(string hash)
    {
        var items = _dbSet.First(l => l.Hash == hash);
        return items;
    }

    public List<ConnectionParams> GetAllConnectionsByHash(string hash)
    {
        var items = _dbSet.Where(p => p.Hash == hash).ToList();
        return items;
    }
    public List<ConnectionParams> GetAllConnectionsByIpAddr(string ip)
    {
        var items = _dbSet.Where(p => p.RemoteIpAddr == ip).ToList();
        return items;
    }
}