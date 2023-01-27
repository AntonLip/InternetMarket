using Contributors.Interfaces.Repository;
using InternetMarket.Interfaces.IService;
using InternetMarket.Models.DbModels;
using System;
using System.Collections.Generic;

namespace InternetMarket.Services
{
    public class ShoppingCartService : BaseService<ShoppingCart>, IShoppingCartService
    {
        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
            : base(shoppingCartRepository)
        {

        }

        public List<ShoppingCart> GetUserShoppingCarts(string userId, bool isBying)
        {
            if (userId == string.Empty)
                throw new ArgumentNullException();
            var products = _repository.GetWithInclude(p => p.UserId == userId && p.IsBying == isBying);
            return products is null ? throw new ArgumentNullException() : products;
        }
    }
}
