using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace InternetMarket.Models.ViewModels.HomeViewModels
{
    public class AddProductViewModel
    {
        [Required (ErrorMessage = "Не указано имя")]
        public string Name { get; set; }
        [Required (ErrorMessage = "Не указан артикул")]
        public string ArticleNumber { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int Coast { get; set; }
        public Guid TypeId{ get; set; }
        public IFormFile Photo { get; set; } 
    }
}
