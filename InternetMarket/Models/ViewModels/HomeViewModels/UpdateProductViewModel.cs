using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace InternetMarket.Models.ViewModels.HomeViewModels
{
    public class UpdateProductViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string ArticleNumber { get; set; }
        public string Description { get; set; }
        [Required]
        public int Count { get; set; }
        public int Coast { get; set; }
        public Guid TypeId { get; set; }
        public IFormFile Photo { get; set; }
    }
}
