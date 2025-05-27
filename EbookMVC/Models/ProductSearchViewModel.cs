using Microsoft.AspNetCore.Mvc.Rendering;

namespace EbookMVC.Models
{
    public class ProductSearchViewModel
    {
        // Tham số tìm kiếm
        public string Keyword { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Level { get; set; }
        
        // Kết quả tìm kiếm
        public IEnumerable<Product> Products { get; set; }
        
        // Dữ liệu cho dropdown
        public SelectList Categories { get; set; }
        public SelectList Levels { get; set; }
        
        // Trạng thái tìm kiếm
        public bool IsSearched { get; set; }
    }
}