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
}