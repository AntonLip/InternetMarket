using System;
using System.Collections.Generic;

namespace InternetMarket.Models.ViewModels.HomeViewModels
{
    public class ShoppingСartViewModel
    {
        public List<ShoppingCartDto> ShoppingСarts { get; set; }
        public int TotalCost { get; set; }
    }
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string ArticleNumber { get; set; }
        public int Cost { get; set; }
        public int Count { get; set; }
        public string BuyDate { get; set; }
    }

}
