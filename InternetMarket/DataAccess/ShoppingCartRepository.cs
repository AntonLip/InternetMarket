using Contributors.Interfaces.Repository;
using InternetMarket.DataAccess;
using InternetMarket.Models.DbModels;

namespace Contributors.DataAccess
{
    internal class ShoppingCartRepository : BaseRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(AppDbContext context)
            : base(context)
        {

        }
    }
}
