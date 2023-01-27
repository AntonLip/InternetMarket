using InternetMarket.Models.ViewModels.HomeViewModels;
using System.Collections.Generic;

namespace InternetMarket.Models.ViewModels.AdminViewModels
{
    public class UsersProductsViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<ShoppingCartDto> Products { get; set; }
        public int TotalCost { get; set; }
    }
}
