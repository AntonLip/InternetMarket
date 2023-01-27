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
    public class ProductTypeServiceTest
    {
        private Mock<IProductTypeRepository> _productTypeRepository;
        private IProductTypeRepository _mockProductTypeRepository;
        private ProductTypeService _productTypeService;
        private readonly List<ProductType> _fakeProductTypes = new List<ProductType>();

        private static Random _random;
        public ProductTypeServiceTest()
        {
            GenerateData();
        }
        [Fact]
        public ProductType Product_Add_return_ProductType()
        {
            CreateDefaultProductServiceInstance();
            var productType = _productTypeService.Add(_fakeProductTypes[0]);
            Assert.NotNull(productType);
            return productType;
        }
        [Fact]
        public List<ProductType> GetAll_Reeturn_ListProductType()
        {
            CreateDefaultProductServiceInstance();
            var productType = _productTypeService.GetAll();
            Assert.True(Equals(10, productType.Count));
            return productType;
        }
        [Fact]
        public ProductType Guid_GetById_Return_ProductType()
        {
            CreateDefaultProductServiceInstance();
            var productType = _productTypeService.GetById(_fakeProductTypes[0].Id);
            Assert.NotNull(productType);
            return productType;
        }
        [Fact]
        public ProductType Guid_Remove_Return_ProductType()
        {
            CreateDefaultProductServiceInstance();
            var productType = _productTypeService.Remove(_fakeProductTypes[0].Id);
            Assert.NotNull(productType);
            return productType;
        }
        [Fact]
        public ProductType Guid_Update_Return_ProductType()
        {
            CreateDefaultProductServiceInstance();
            var productType = _productTypeService.Update(_fakeProductTypes[0].Id, _fakeProductTypes[0]);
            Assert.NotNull(productType);
            return productType;
        }
       

        private void CreateDefaultProductServiceInstance()
        {
            _productTypeRepository = new Mock<IProductTypeRepository>();
            _productTypeRepository.Setup(s => s.GetAll()).Returns(_fakeProductTypes);
            _productTypeRepository.Setup(s => s.GetById(It.IsAny<Guid>())).Returns(_fakeProductTypes[1]);
            _productTypeRepository.Setup(s => s.Remove(It.IsAny<ProductType>()));
            _productTypeRepository.Setup(s => s.Add(It.IsAny<ProductType>()));
            _productTypeRepository.Setup(s => s.Update(It.IsAny<ProductType>()));
            _mockProductTypeRepository = _productTypeRepository.Object;
            _productTypeService = new ProductTypeService(_mockProductTypeRepository);
        }

        private void GenerateData()
        {
            _random = new Random();

            for (int i = 0; i < 10; i++)
            {
                _fakeProductTypes.Add(
                    new ProductType
                    {
                        Id = Guid.NewGuid(),
                        Name = RandomString(12)
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
