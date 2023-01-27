using InternetMarket.Models.DbModels;
using InternetMarket.Models.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;

namespace InternetMarket.Interfaces.IService
{
    public interface IProductService : IServices<Product, Guid>
    {
        List<Product> GetByType(Guid typeId);
        FileDto GetReport(UsersProductsViewModel model);
        List<Product> Find(string stringFind);
    }
}
