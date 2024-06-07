using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Controllers;
using ProductApi.Models;
using ProductApi.Repositories;
using Xunit;

namespace ProductApi.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void GetProducts_Returns_Products()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var expectedProducts = new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Product 1", Price = 130.00m },
                new Product { ProductId = 2, ProductName = "Product 2", Price = 210.00m }
            };
            mockRepository.Setup(repo => repo.GetAll()).Returns(expectedProducts);
            var controller = new ProductController(mockRepository.Object);

            // Act
            var result = controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal(expectedProducts, products);
        }

        [Fact]
        public void GetProduct_Returns_Product_With_Valid_Id()
        {
            // Arrange
            var expectedProduct = new Product { ProductId = 1, ProductName = "Product 1", Price = 10.00m };
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetById(1)).Returns(expectedProduct);
            var controller = new ProductController(mockRepository.Object);

            // Act
            var result = controller.GetProduct(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(expectedProduct, product);
        }

        [Fact]
        public void GetProduct_Returns_NotFound_With_Invalid_Id()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Product)null);
            var controller = new ProductController(mockRepository.Object);

            // Act
            var result = controller.GetProduct(100); // Assuming this ID does not exist

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
