using CloudOrder.Business.DTOs.Products;
using CloudOrder.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudOrder.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await _productService.GetProductAsync(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto request)
    {
        var created = await _productService.CreateProductAsync(request);
        return CreatedAtAction(nameof(GetProduct), new { id = created.Id }, created);
    }
}
