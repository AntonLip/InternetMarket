using InternetMarket.Interfaces.Repository;
using InternetMarket.Models.DbModels;
using System;

namespace Contributors.Interfaces.Repository
{
    public interface IProductTypeRepository : IRepository<ProductType, Guid>
    {
    }
}
