using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using RepostoryLayer;
using ServiceLayer.Repostory;
using System.Threading.Tasks;
using Xunit;

namespace TestingServices
{
	public class TestingService
	{
		[Fact]
		public async Task AddAsync_Should_Add_Product()
		{
			// Arrange
			var products = new List<Products>();
			var productsDbSetMock = new Mock<DbSet<Products>>();
			var context = new Mock<AppDbContext>();

			context.Setup(c => c.Products).Returns(productsDbSetMock.Object);
			productsDbSetMock.Setup(p => p.AddAsync(It.IsAny<Products>(), default))
				.Callback<Products, System.Threading.CancellationToken>((product, token) =>
				{
					products.Add(product);
				})
				.Returns<Products, System.Threading.CancellationToken>((product, token) =>
				{
					return Task.FromResult(product);
				});

			var repository = new ProductRepostory(context.Object);

			var product = new Products
			{
				// Set the properties of the product here
			};

			// Act
			var addedProduct = await repository.AddAsync(product);

			// Assert
			Assert.NotNull(addedProduct);
			Assert.Contains(addedProduct, products);
		}

		[Fact]
		public async Task GetAllAsync_Should_Return_All_Products()
		{
			// Arrange
			var products = new List<Products>
			{
				new Products { ProductCode = 1, Name = "Product 1" },
				new Products { ProductCode = 2, Name = "Product 2" },
				new Products { ProductCode = 3, Name = "Product 3" },
			};

			var context = new Mock<AppDbContext>();
			context.Setup(c => c.Products.ToListAsync(default)).Returns(Task.FromResult(products));
			var repository = new ProductRepostory(context.Object);

			// Act
			var allProducts = await repository.GetAllAsync();

			// Assert
			Assert.NotNull(allProducts);
			Assert.Equal(3, allProducts.Count);
		}

		[Fact]
		public async Task GetByDeleteAsync_Should_Delete_Product()
		{
			// Arrange
			var productToDelete = new Products { ProductCode = 1, Name = "Product 1" };
			var context = new Mock<AppDbContext>();
			context.Setup(c => c.Products.FindAsync(1)).Returns(Task.FromResult(productToDelete));
			var repository = new ProductRepostory(context.Object);

			// Act
			var deletedProduct = await repository.GetByDeleteAsync(1);

			// Assert
			Assert.NotNull(deletedProduct);
			context.Verify(c => c.Remove(productToDelete), Times.Once);
			context.Verify(c => c.SaveChangesAsync(default), Times.Once);
		}

		[Fact]
		public async Task GetByDeleteAsync_Should_Return_Null_When_Product_Not_Found()
		{
			// Arrange
			var context = new Mock<AppDbContext>();
			context.Setup(c => c.Products.FindAsync(1)).Returns(Task.FromResult((Products)null));
			var repository = new ProductRepostory(context.Object);

			// Act
			var deletedProduct = await repository.GetByDeleteAsync(1);

			// Assert
			Assert.Null(deletedProduct);
			context.Verify(c => c.Remove(It.IsAny<Products>()), Times.Never);
			context.Verify(c => c.SaveChangesAsync(default), Times.Never);
		}

		[Fact]
		public async Task GetByIdAsync_Should_Return_Product()
		{
			// Arrange
			var product = new Products { ProductCode = 1, Name = "Product 1" };
			var context = new Mock<AppDbContext>();
			context.Setup(c => c.Products.FindAsync(1)).Returns(Task.FromResult(product));
			var repository = new ProductRepostory(context.Object);

			// Act
			var retrievedProduct = await repository.GetByIdAsync(1);

			// Assert
			Assert.NotNull(retrievedProduct);
			Assert.Equal(1, retrievedProduct.ProductCode);
			Assert.Equal("Product 1", retrievedProduct.Name);
		}

		[Fact]
		public async Task GetByIdAsync_Should_Return_Null_When_Product_Not_Found()
		{
			// Arrange
			var context = new Mock<AppDbContext>();
			context.Setup(c => c.Products.FindAsync(1)).Returns(Task.FromResult((Products)null));
			var repository = new ProductRepostory(context.Object);

			// Act
			var retrievedProduct = await repository.GetByIdAsync(1);

			// Assert
			Assert.Null(retrievedProduct);
		}

		[Fact]
		public async Task GetByUpdateAsync_Should_Update_Product()
		{
			// Arrange
			var product = new Products { ProductCode = 1, Name = "Product 1" };
			var context = new Mock<AppDbContext>();
			var dbSetMock = new Mock<DbSet<Products>>();
			context.Setup(c => c.Products).Returns(dbSetMock.Object);
			dbSetMock.Setup(d => d.Update(It.IsAny<Products>()));
			context.Setup(c => c.SaveChangesAsync(default)).Returns(Task.FromResult(1));
			var repository = new ProductRepostory(context.Object);

			// Act
			var updatedProduct = await repository.GetByUpdateAysnc(product);

			// Assert
			Assert.NotNull(updatedProduct);
			dbSetMock.Verify(d => d.Update(product), Times.Once);
			context.Verify(c => c.SaveChangesAsync(default), Times.Once);
		}
	}
}
