using InternetMarket.Interfaces;
using System;
using System.Collections.Generic;

namespace InternetMarket.Models.DbModels
{
    public class ProductType : IEntity<Guid>
    {
        public  Guid Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}