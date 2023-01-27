using Contributors.Interfaces.Repository;
using InternetMarket.Models.DbModels;
using InternetMarket.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace InternetMarket.Tests
{
    public class ShoppingCartServiceTest
    {
        private Mock<IShoppingCartRepository> _productRepository;
        private IShoppingCartRepository _mockProductRepository;
        private ShoppingCartService _ShoppingCartService;
        private List<ShoppingCart> _fakeShoppingCarts = new List<ShoppingCart>();

        private static Random _random;
        public ShoppingCartServiceTest()
        {
            GenerateData();
        }
        [Fact]
        public ShoppingCart Product_Add_return_Product()
        {
            CreateDefaultShoppingCartServiceInstance();
            var shoppingCart = _ShoppingCartService.Add(_fakeShoppingCarts[0]);
            Assert.NotNull(shoppingCart);
            return shoppingCart;
        }
        [Fact]
        public List<ShoppingCart> GetAll_Reeturn_ListShoppingCart()
        {
            CreateDefaultShoppingCartServiceInstance();
            var shoppingCart = _ShoppingCartService.GetAll();
            Assert.True(Equals(10, shoppingCart.Count));
            return shoppingCart;
        }
        [Fact]
        public ShoppingCart Guid_GetById_Return_Product()
        {
            CreateDefaultShoppingCartServiceInstance();
            var shoppingCart = _ShoppingCartService.GetById(_fakeShoppingCarts[0].Id);
            Assert.NotNull(shoppingCart);
            return shoppingCart;
        }
        [Fact]
        public ShoppingCart Guid_Remove_Return_ShoppingCart()
        {
            CreateDefaultShoppingCartServiceInstance();
            var shoppingCart = _ShoppingCartService.Remove(_fakeShoppingCarts[0].Id);
            Assert.NotNull(shoppingCart);
            return shoppingCart;
        }
        [Fact]
        public ShoppingCart Guid_Update_Return_ShoppingCart()
        {
            CreateDefaultShoppingCartServiceInstance();
            var shoppingCart = _ShoppingCartService.Update(_fakeShoppingCarts[0].Id, _fakeShoppingCarts[0]);
            Assert.NotNull(shoppingCart);
            return shoppingCart;
        }

        private void CreateDefaultShoppingCartServiceInstance()
        {
            _productRepository = new Mock<IShoppingCartRepository>();
            _productRepository.Setup(s => s.GetAll()).Returns(_fakeShoppingCarts);
            _productRepository.Setup(s => s.GetById(It.IsAny<Guid>())).Returns(_fakeShoppingCarts[1]);
            _productRepository.Setup(s => s.Remove(It.IsAny<ShoppingCart>()));
            _productRepository.Setup(s => s.Add(It.IsAny<ShoppingCart>()));
            _productRepository.Setup(s => s.Update(It.IsAny<ShoppingCart>()));
            
            _mockProductRepository = _productRepository.Object;
            _ShoppingCartService = new ShoppingCartService(_mockProductRepository);
        }

        private void GenerateData()
        {
            _random = new Random();

            for (int i = 0; i < 10; i++)
            {
                _fakeShoppingCarts.Add(
                    new ShoppingCart
                    {
                        BuyDate = DateTime.Now,
                        Cost = _random.Next(1, 16532145),
                        Count = _random.Next(1, 20),
                        UserId = Guid.NewGuid().ToString(),
                        IsBying = false,
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid()
                    });
            }

        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
