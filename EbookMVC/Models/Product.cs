using System.ComponentModel.DataAnnotations;

namespace EbookMVC.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Tên khóa học")]
        public string Name { get; set; }

        [Range(0.01, 10000000.00)]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [Display(Name = "Danh mục")]
        public Category Category { get; set; }

        [Display(Name = "Thời lượng (giờ)")]
        public double Duration { get; set; }

        [Display(Name = "Cấp độ")]
        public string Level { get; set; }
    }
}
