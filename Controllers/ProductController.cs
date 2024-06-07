using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Repositories;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a list of products.
        /// </summary>
        /// <returns>A list of products.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _repository.GetAll();
            return Ok(products);
        }

        /// <summary>
        /// Retrieves details of a specific product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>Details of the specified product.</returns>
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
                return NotFound(new { Message = $"Product with ID {id} not found." });

            return Ok(product);
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>The added product.</returns>
        [HttpPost]
        public ActionResult<Product> AddProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest(new { Message = "Product cannot be null." });

            _repository.Add(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        /// <summary>
        /// Updates details of a specific product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="product">The updated details of the product.</param>
        /// <returns>The updated product.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null)
                return BadRequest(new { Message = "Product cannot be null." });

            if (id != product.ProductId)
                return BadRequest(new { Message = "ID mismatch." });

            var existingProduct = _repository.GetById(id);
            if (existingProduct == null)
                return NotFound(new { Message = $"Product with ID {id} not found." });

            _repository.Update(product);
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
                return NotFound(new { Message = $"Product with ID {id} not found." });

            _repository.Delete(id);
            return NoContent();
        }
    }
}
