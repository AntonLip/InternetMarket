using Contributors.Interfaces.Repository;
using InternetMarket.Interfaces.IService;
using InternetMarket.Models.DbModels;


namespace InternetMarket.Services
{
    public class ProductTypeService : BaseService<ProductType>, IProductTypeService
    {
        public ProductTypeService(IProductTypeRepository productTypeRepository)
            :base(productTypeRepository)
        {

        }
    }
}
