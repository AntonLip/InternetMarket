using InternetMarket.Models.DbModels;
using System.Collections.Generic;

namespace InternetMarket.Models.ViewModels.HomeViewModels
{
    public class IndexViewModel
    {
        public List<Product> Products { get; set; }
        public List<ProductType> ProductTypes { get; set; }
    }
}
