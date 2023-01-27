using InternetMarket.Interfaces.Repository;
using InternetMarket.Models.DbModels;
using System;
using System.Collections.Generic;

namespace Contributors.Interfaces.Repository
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
        List<Product> Find(string stringFind);
    }
}
