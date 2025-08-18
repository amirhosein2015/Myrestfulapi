using Microsoft.AspNetCore.Mvc;

namespace MyRestfulApi.Controllers
{
    // Controllers/ProductsController.cs
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using MyRestfulApi.Models;

    [Authorize] // این خط یعنی تمام اکشن‌های این کنترلر نیاز به توکن دارند
    [ApiController]
    [ApiVersion("1.0")] // مشخص کردن نسخه این کنترلر
    [ApiVersion("2.0")] //  به کنترلر می‌گوییم که از نسخه ۲ هم پشتیبانی می‌کند
    [Route("api/v{version:apiVersion}/[controller]")] // آدرس RESTful با نسخه
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/products
        [HttpGet]
        [MapToApiVersion("1.0")] // این متد را فقط برای نسخه ۱ مپ می‌کنیم
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }




        // GET: api/v2/products
        [HttpGet]
        [MapToApiVersion("2.0")] // یک متد جدید فقط برای نسخه ۲ می‌سازیم
        public async Task<IActionResult> GetProductsV2()
        {
            var products = await _context.Products.ToListAsync();

            // ۴. یک تغییر کوچک در ساختار پاسخ ایجاد می‌کنیم تا تفاوت مشخص شود
            var response = new
            {
                ApiVersion = "2.0",
                Count = products.Count,
                Products = products
            };

            return Ok(response);
        }

        // متدهای GET (by Id), POST, PUT, DELETE هم به همین شکل می‌توانند نسخه‌بندی شوند
        // فعلاً برای سادگی، آن‌ها را برای نسخه ۱ در نظر می‌گیریم









        // GET: api/v1/products/5
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/v1/products
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            product.CreatedDate = DateTime.UtcNow;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id, version = "1.0" }, product);
        }

        // ... متدهای PUT و DELETE هم به همین شکل اضافه می‌شوند ...
    }
}
