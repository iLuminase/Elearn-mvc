using System.ComponentModel.DataAnnotations;

namespace EbookMVC.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}
