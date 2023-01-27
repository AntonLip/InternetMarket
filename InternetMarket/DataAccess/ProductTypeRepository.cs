using Contributors.Interfaces.Repository;
using InternetMarket.DataAccess;
using InternetMarket.Models.DbModels;

namespace Contributors.DataAccess
{
    class ProductTypeRepository : BaseRepository<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(AppDbContext context)
            : base(context)
        {

        }
    }
}
