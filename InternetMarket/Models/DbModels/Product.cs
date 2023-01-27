using InternetMarket.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternetMarket.Models.DbModels
{
    public class Product : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int Coast { get; set; }
        public string ArticleNumber { get; set; }
        public string PisturePath { get; set; }
        [ForeignKey("LessonType")]
        public Guid ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
    }
}
