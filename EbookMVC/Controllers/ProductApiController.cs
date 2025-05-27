using Microsoft.AspNetCore.Mvc;
using EbookMVC.Models;
using EbookMVC.Repository;

namespace EbookMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductApiController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // API endpoint cho tìm kiếm real-time
        [HttpGet("search")]
        public IActionResult Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return Ok(new { success = false, data = new List<object>() });
            }

            var products = _productRepository.Search(keyword);
            var result = products.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                price = p.Price.ToString("N0") + " VNĐ",
                categoryName = p.Category?.Name ?? "Chưa phân loại",
                imageUrl = p.ImageUrl ?? "",
                level = p.Level ?? ""
            }).ToList();

            return Ok(new { success = true, data = result });
        }
    }
}
