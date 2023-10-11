using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

[TestClass]
public class ProductControllerIntegrationTests
{
	private HttpClient _client;

	[TestInitialize]
	public void Setup()
	{
		// Initialize your HttpClient with the base address of your API
		_client = new HttpClient();
		_client.BaseAddress = new Uri("http://localhost:5000"); // Replace with your API address
	}

	[TestMethod]
	public async Task GetAllProduct_ShouldReturnProducts()
	{
		// Arrange

		// Act
		var response = await _client.GetAsync("/api/Product");

		// Assert
		Assert.IsTrue(response.IsSuccessStatusCode);
		// Add more assertions to validate the response content
	}

	[TestCleanup]
	public void Cleanup()
	{
		// Clean up resources if needed
		_client.Dispose();
	}
}
