using InternetMarket.Models.DbModels;
using System;
using System.Collections.Generic;

namespace InternetMarket.Interfaces.IService
{
    public interface IShoppingCartService : IServices<ShoppingCart, Guid>
    {
        List<ShoppingCart> GetUserShoppingCarts(string userId, bool isBying);
    }
}
