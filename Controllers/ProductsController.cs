using Gigashop.Data;
using Gigashop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gigashop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        // Constructor để inject DbContext
        public ProductsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }


        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // Tìm sản phẩm theo ID
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(); // Trả về lỗi nếu không tìm thấy sản phẩm
            }

            return Ok(product); // Trả về sản phẩm dưới dạng JSON
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // Thêm sản phẩm mới vào cơ sở dữ liệu
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest(); // Kiểm tra ID có khớp hay không
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductID == id))
                {
                    return NotFound(); // Nếu không tìm thấy sản phẩm
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Trả về 204 No Content nếu thành công
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(); // Trả về lỗi nếu không tìm thấy sản phẩm
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent(); // Trả về 204 No Content nếu xóa thành công
        }
      

    }
}
