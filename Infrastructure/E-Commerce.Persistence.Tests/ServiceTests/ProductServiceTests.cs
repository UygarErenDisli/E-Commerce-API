using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Exceptions;
using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
namespace E_Commerce.Persistence.Tests.ServicesTests
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        public ProductServiceTests()
        {
            _productReadRepository = A.Fake<IProductReadRepository>();
            _productWriteRepository = A.Fake<IProductWriteRepository>();
            _productImageFileWriteRepository = A.Fake<IProductImageFileWriteRepository>();

            _productService = new ProductService(_productReadRepository,
                                                 _productWriteRepository,
                                                 _productImageFileWriteRepository);
        }

        [Fact]
        public void GetProducts_Returns_Different_Products_For_Each_Page()
        {
            // Arrange

            var mockProductsInDb = new List<Product>() {
                new()
                    {
                        Id = Guid.NewGuid(),
                        Name ="Test Product 1",
                        Price = 1,
                        Stock = 1,
                    },
                new()
                    {
                        Id = Guid.NewGuid(),
                        Name ="Test Product 2",
                        Price = 2,
                        Stock = 2,
                    },
                new()
                    {
                        Id = Guid.NewGuid(),
                        Name ="Test Product 3",
                        Price = 3,
                        Stock = 3,
                    },
                new()
                    {
                        Id = Guid.NewGuid(),
                        Name ="Test Product 4",
                        Price = 4,
                        Stock = 4,
                    },
                new()
                    {
                        Id = Guid.NewGuid(),
                        Name ="Test Product 5",
                        Price = 5,
                        Stock = 5,
                    },
                new()
                    {
                        Id = Guid.NewGuid(),
                        Name ="Test Product 6",
                        Price = 6,
                        Stock = 6,
                    },
            };
            int page = 0;
            int pageSize = 2;
            A.CallTo(() => _productReadRepository.GetAll(A<bool>.Ignored)).Returns(mockProductsInDb.AsQueryable());

            // Act

            var firstPage = _productService.GetProducts(page, pageSize);
            var secondPage = _productService.GetProducts(page + 1, pageSize);

            // Assert

            firstPage.Should().NotBeEmpty().And.HaveCount(pageSize);
            secondPage.Should().NotBeEmpty().And.HaveCount(pageSize);
            firstPage.Select(p => p.Id).Intersect(secondPage.Select(p => p.Id)).Any().Should().BeFalse("Because returned pages shouldn't contain same products!");
        }

        [Fact]
        public async void GetProductById_Returns_A_Product()
        {
            // Arrange
            var fakeProduct = A.Fake<Product>();
            A.CallTo(() => _productReadRepository.GetByIdAsync(A<string>.Ignored, A<bool>.Ignored))
                        .Returns(fakeProduct);
            // Act
            var result = await _productService.GetProductByIdAsync("Test Id");

            // Assert
            result.Should().NotBeOfType<Product>();
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void GetProductById_Should_Throw_An_ArgumentException_For_Invalid_Ids(string id)
        {
            // Arrange
            // Act
            var actionForInvalidId = async () => await _productService.GetProductByIdAsync(id);

            // Assert
            await actionForInvalidId.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void GetProductById_Should_Throw_An_ProductNotFoundException_For_Null_Product()
        {
            // Arrange
            Product? nullProduct = null;
            A.CallTo(() => _productReadRepository.GetByIdAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(nullProduct);
            // Act
            var actionForNullProject = async () => await _productService.GetProductByIdAsync("Test Id");

            // Assert
            await actionForNullProject.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task CreateProduct_Should_Return_True_On_Success()
        {
            // Arrange
            A.CallTo(() => _productWriteRepository.AddAsync(A<Product>.Ignored))
                .Returns(true);

            // Act
            var result = await _productService.CreateProductAsync(A.Fake<CreateProductDTO>());

            // Assert
            result.Should().BeTrue();
            A.CallTo(() => _productWriteRepository.SaveAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _productWriteRepository.AddAsync(A<Product>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CreateProduct_Should_Return_False_On_Failure()
        {
            // Arrange
            A.CallTo(() => _productWriteRepository.AddAsync(A<Product>.Ignored))
                .Returns(false);

            // Act
            var result = await _productService.CreateProductAsync(A.Fake<CreateProductDTO>());

            // Assert
            result.Should().BeFalse();
            A.CallTo(() => _productWriteRepository.SaveAsync()).MustNotHaveHappened();
        }

        [Fact]
        public async Task CreateProduct_Should_Throw_An_ProductNotFoundException_For_Null_Model()
        {

            //Arrange
            CreateProductDTO? nullModel = null;

            // Act
            var actionForNullModel = async () => await _productService.CreateProductAsync(nullModel);

            // Arrange
            await actionForNullModel.Should().ThrowAsync<ArgumentException>();
            A.CallTo(() => _productWriteRepository.SaveAsync()).MustNotHaveHappened();
            A.CallTo(() => _productWriteRepository.AddAsync(A<Product>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task DeleteProduct_Should_Return_True_On_Success()
        {
            // Arrange
            var returnProduct = new Product();
            A.CallTo(() => _productReadRepository.GetByIdAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(returnProduct);
            A.CallTo(() => _productWriteRepository.Remove(A<Product>.Ignored)).Returns(true);

            // Act

            var result = await _productService.DeleteProductByIdAsync("Test Product");

            // Arrange

            result.Should().BeTrue();
            A.CallTo(() => _productWriteRepository.SaveAsync()).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async Task DeleteProduct_Should_Return_False_On_Failure()
        {

            // Arrange
            var returnedProduct = new Product();
            A.CallTo(() => _productReadRepository.GetByIdAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(A.Dummy<Product>());
            A.CallTo(() => _productWriteRepository.Remove(A<Product>.Ignored)).Returns(false);

            // Act
            var result = await _productService.DeleteProductByIdAsync("Test Product");

            // Arrange
            result.Should().BeFalse();
            A.CallTo(() => _productWriteRepository.SaveAsync()).MustNotHaveHappened();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task DeleteProductById_Should_Throw_An_ArgumentException_For_Invalid_Ids(string id)
        {
            //Arrange

            // Act
            var actionForInvalidId = async () => await _productService.DeleteProductByIdAsync(id);
            // Arrange

            await actionForInvalidId.Should().ThrowAsync<ArgumentException>();
            A.CallTo(() => _productWriteRepository.SaveAsync()).MustNotHaveHappened();
            A.CallTo(() => _productWriteRepository.Remove(A<Product>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task DeleteProductById_Should_Throw_An_ProductNotFoundException_For_Null_Product()
        {

            //Arrange
            Product? nullModel = null;
            A.CallTo(() => _productReadRepository.GetByIdAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(nullModel);

            // Act
            var actionForNullModel = async () => await _productService.DeleteProductByIdAsync("Test Product");

            // Arrange
            await actionForNullModel.Should().ThrowAsync<ProductNotFoundException>();
            A.CallTo(() => _productWriteRepository.SaveAsync()).MustNotHaveHappened();
            A.CallTo(() => _productWriteRepository.Remove(A<Product>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task UpdateProduct_Should_Return_True_On_Sucess()
        {
            // Arrange
            A.CallTo(() => _productReadRepository.GetByIdAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(A.Dummy<Product>());
            A.CallTo(() => _productWriteRepository.Update(A<Product>.Ignored)).Returns(true);
            // Act
            var result = await _productService.UpdateProductAsync(A.Dummy<UpdateProductDTO>());
            // Arrange
            result.Should().BeTrue();
            A.CallTo(() => _productWriteRepository.SaveAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _productReadRepository.GetByIdAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _productWriteRepository.Update(A<Product>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateProduct_Should_Return_False_On_Failure()
        {

            // Arrange
            var returnedProduct = new Product();
            A.CallTo(() => _productReadRepository.GetByIdAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(A.Dummy<Product>());
            A.CallTo(() => _productWriteRepository.Update(A<Product>.Ignored)).Returns(false);

            // Act
            var result = await _productService.UpdateProductAsync(A.Dummy<UpdateProductDTO>());

            // Arrange
            result.Should().BeFalse();
            A.CallTo(() => _productWriteRepository.SaveAsync()).MustNotHaveHappened();
            A.CallTo(() => _productReadRepository.GetByIdAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _productWriteRepository.Update(A<Product>.Ignored)).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async Task UpdateProduct_Should_Throw_An_ProductNotFoundException_For_Null_Product()
        {

            //Arrange
            Product? nullModel = null;
            A.CallTo(() => _productReadRepository.GetByIdAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(nullModel);

            // Act
            var actionForNullModel = async () => await _productService.UpdateProductAsync(A.Dummy<UpdateProductDTO>());

            // Arrange
            await actionForNullModel.Should().ThrowAsync<ProductNotFoundException>();
            A.CallTo(() => _productReadRepository.GetByIdAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _productWriteRepository.SaveAsync()).MustNotHaveHappened();
            A.CallTo(() => _productWriteRepository.Update(A<Product>.Ignored)).MustNotHaveHappened();
        }

    }
}