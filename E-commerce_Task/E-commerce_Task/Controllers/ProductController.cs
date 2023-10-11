using DomainLayer.DTOS;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.IRepostory;
namespace E_commerce_Task.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IGeneric<Products> _ProductRepostory;

		public ProductController(IGeneric<Products> ProductRepository)
		{
			_ProductRepostory = ProductRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProduct()
		{
			try
			{
				var getAllProduct = await _ProductRepostory.GetAllAsync();
				if (getAllProduct == null)
				{
					return NotFound();
				}
				return Ok(getAllProduct);
			}
			catch (Exception ex)
			{
				return BadRequest($"Empty Product: {ex.Message}");
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var getProductById = await _ProductRepostory.GetByIdAsync(id);
				if (getProductById == null)
				{
					return NotFound();
				}
				return Ok(getProductById);
			}
			catch (Exception ex)
			{
				return BadRequest($"Not Found Product for this id {ex.Message}");
			}
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddProduct([FromForm] AddProductDto addProductDtos, [FromForm] FileUploadImage model)
		{
			try
			{
				var product = new Products
				{
					Name = addProductDtos.Name,
					Categories = addProductDtos.Categories,
					Price = addProductDtos.Price,
					MinimumQuantity = addProductDtos.MinimumQuantity,
					DiscountRate = addProductDtos.DiscountRate,
				};

				if (model.File == null || model.File.Length == 0)
				{
					return BadRequest("No file uploaded.");
				}

				var uploadDirectory = @"D://Project_Freelance//E-commerce_Task//wwwroot//uploads";

				Directory.CreateDirectory(uploadDirectory);

				var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;

				var filePath = Path.Combine(uploadDirectory, uniqueFileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await model.File.CopyToAsync(stream);
				}

				// Set the ImageFilePath property of the product to the file path
				product.image = filePath;

				// Add the product to the repository
				await _ProductRepostory.AddAsync(product);

				return Ok("Product added successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal Server Error: {ex.Message}");
			}
		}

		[HttpPut("id")]
		[Authorize]
		public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDtos updateProduct, [FromForm] FileUploadImage model)
		{
			try
			{
				// Get the existing product by ID
				var existingProduct = await _ProductRepostory.GetByIdAsync(id);

				if (existingProduct == null)
				{
					return BadRequest("Product not found for this ID");
				}

				// Update the properties of the existing product
				existingProduct.Name = updateProduct.Name;
				existingProduct.Categories = updateProduct.Categories;
				existingProduct.Price = updateProduct.Price;
				existingProduct.MinimumQuantity = updateProduct.MinimumQuantity;

				if (model.File != null && model.File.Length > 0)
				{
					var uploadDirectory = "wwwroot/uploads";
					Directory.CreateDirectory(uploadDirectory);
					var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
					var filePath = Path.Combine(uploadDirectory, uniqueFileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await model.File.CopyToAsync(stream);
					}

					// Update the image path of the existing product
					existingProduct.image = filePath;
				}

				// Update the product in the repository
				await _ProductRepostory.GetByUpdateAysnc(existingProduct);

				return Ok("Product updated successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal Server Error: {ex.Message}");
			}
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			try
			{
				var deleteProduct = await _ProductRepostory.GetByDeleteAsync(id);
				if (deleteProduct == null)
				{
					return NotFound("Product Not Found");
				}
				return Ok("Product Deleted Successfully");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal Server Error: {ex.Message}");
			}
		}
	}
}
