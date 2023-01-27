using InternetMarket.Interfaces;
using System;

namespace InternetMarket.Models.DbModels
{
    public class ShoppingCart : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string UserId { get; set; }
        public int Cost { get; set; }
        public int Count { get; set; }
        public bool IsBying { get; set; }
        public DateTime BuyDate { get; set; }
    }
}
