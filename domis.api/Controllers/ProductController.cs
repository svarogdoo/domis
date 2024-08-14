using domis.api.Models;
using domis.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace domis.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _productService.GetAll();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var product = _productService.GetById(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Product product)
    {
        if (product == null)
            return BadRequest();

        product.Guid = Guid.NewGuid();
        _productService.Add(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Guid }, product);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] Product product)
    {
        if (product == null || id != product.Guid)
            return BadRequest();

        var existingProduct = _productService.GetById(id);
        if (existingProduct == null)
            return NotFound();

        _productService.Update(product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var product = _productService.GetById(id);
        if (product == null)
            return NotFound();

        _productService.Delete(id);
        return NoContent();
    }
}
