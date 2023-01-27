using InternetMarket.Models.DbModels;

namespace InternetMarket.Models.ViewModels.HomeViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public int CountChosen { get; set; }
        public string PageTitle { get; set; }
    }
}
