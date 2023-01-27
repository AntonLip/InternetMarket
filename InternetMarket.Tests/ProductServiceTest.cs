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

    public class ProductServiceTest
    {
        private Mock<IProductRepository> _productRepository;
        private IProductRepository _mockProductRepository;
        private ProductService _productService;
        private readonly List<Product> _fakeProducts = new List<Product>();

        private static Random _random;
        public ProductServiceTest()
        {
            GenerateData();
        }
        [Fact]
        public Product Product_Add_return_Product()
        {
            CreateDefaultProductServiceInstance();
            var product = _productService.Add(_fakeProducts[0]);
            Assert.NotNull(product);
            return product;
        }
        [Fact]
        public List<Product> GetAll_Reeturn_ListProduct()
        {
            CreateDefaultProductServiceInstance();
            var product = _productService.GetAll();
            Assert.True(Equals(10, product.Count));
            return product;
        }
        [Fact]
        public Product Guid_GetById_Return_Product()
        {
            CreateDefaultProductServiceInstance();
            var product = _productService.GetById(_fakeProducts[0].Id);
            Assert.NotNull(product);
            return product;
        }
        [Fact]
        public Product Guid_Remove_Return_Product()
        {
            CreateDefaultProductServiceInstance();
            var product = _productService.Remove(_fakeProducts[0].Id);
            Assert.NotNull(product);
            return product;
        }
        [Fact]
        public Product Guid_Update_Return_Product()
        {
            CreateDefaultProductServiceInstance();
            var product = _productService.Update(_fakeProducts[0].Id, _fakeProducts[0]);
            Assert.NotNull(product);
            return product;
        }
        [Fact]
        public List<Product> String_Find_Return_ListProducts()
        {
            CreateDefaultProductServiceInstance();
            var products = _productService.Find(_fakeProducts[0].Name);
            Assert.True(Equals(10, products.Count));
            return products;
        }

        private void CreateDefaultProductServiceInstance()
        {            
            _productRepository = new Mock<IProductRepository>();
            _productRepository.Setup(s => s.GetAll()).Returns(_fakeProducts);
            _productRepository.Setup(s => s.GetById(It.IsAny<Guid>())).Returns(_fakeProducts[1]);
            _productRepository.Setup(s => s.Remove(It.IsAny<Product>()));
            _productRepository.Setup(s => s.Add(It.IsAny<Product>()));
            _productRepository.Setup(s => s.Update(It.IsAny<Product>()));
            _productRepository.Setup(s => s.Find(It.IsAny<string>())).Returns(_fakeProducts);
            _mockProductRepository = _productRepository.Object;
            _productService = new ProductService(_mockProductRepository);
        }

        private void GenerateData()
        {
            _random = new Random();

            for (int i = 0; i < 10; i++)
            {
                _fakeProducts.Add(
                    new Product
                    {
                        ArticleNumber = RandomString(10),
                        Description = RandomString(50),
                        Name = RandomString(5),
                        Coast = _random.Next(20,265000),
                        Count = _random.Next(1, 162300),
                        Id = Guid.NewGuid(),
                        ProductTypeId = Guid.NewGuid()
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
