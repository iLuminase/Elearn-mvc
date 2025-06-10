using Microsoft.AspNetCore.Identity;

namespace EbookMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Có thể thêm các thuộc tính tùy chỉnh ở đây
        public string? FullName { get; set; }
        // Ví dụ:
        // public string? FirstName { get; set; }
        // public string? LastName { get; set; }
        // public DateTime? DateOfBirth { get; set; }
    }
}
