using EbookMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EbookMVC.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            // Đảm bảo database đã được tạo
            context.Database.EnsureCreated();

            // Tạo user mặc định nếu chưa có
            await CreateDefaultUser(userManager);

            // Kiểm tra xem đã có dữ liệu chưa
            if (context.Categories.Any())
            {
                return; // Đã có dữ liệu
            }

            // Thêm Categories
            var categories = new Category[]
            {
                new Category
                {
                    Name = "Lập trình Web",
                    Description = "Các khóa học về phát triển web frontend và backend"
                },
                new Category
                {
                    Name = "Lập trình Mobile",
                    Description = "Các khóa học phát triển ứng dụng di động"
                },
                new Category
                {
                    Name = "Data Science",
                    Description = "Các khóa học về khoa học dữ liệu và machine learning"
                },
                new Category
                {
                    Name = "DevOps",
                    Description = "Các khóa học về triển khai và vận hành hệ thống"
                }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Thêm Products
            var products = new Product[]
            {
                new Product
                {
                    Name = "ASP.NET Core MVC Cơ bản",
                    Description = "Học cách xây dựng ứng dụng web với ASP.NET Core MVC từ cơ bản đến nâng cao",
                    Price = 1500000,
                    CategoryId = 1,
                    Duration = 40,
                    Level = "Cơ bản",
                    ImageUrl = "/images/aspnet-core.jpg"
                },
                new Product
                {
                    Name = "React.js cho người mới bắt đầu",
                    Description = "Khóa học React.js từ zero đến hero, bao gồm hooks và context API",
                    Price = 1200000,
                    CategoryId = 1,
                    Duration = 35,
                    Level = "Cơ bản",
                    ImageUrl = "/images/reactjs.jpg"
                },
                new Product
                {
                    Name = "Flutter App Development",
                    Description = "Phát triển ứng dụng mobile đa nền tảng với Flutter và Dart",
                    Price = 1800000,
                    CategoryId = 2,
                    Duration = 50,
                    Level = "Trung cấp",
                    ImageUrl = "/images/flutter.jpg"
                },
                new Product
                {
                    Name = "Python Data Analysis",
                    Description = "Phân tích dữ liệu với Python, Pandas, NumPy và Matplotlib",
                    Price = 2000000,
                    CategoryId = 3,
                    Duration = 45,
                    Level = "Trung cấp",
                    ImageUrl = "/images/python-data.jpg"
                },
                new Product
                {
                    Name = "Docker và Kubernetes",
                    Description = "Container hóa ứng dụng và quản lý với Docker và Kubernetes",
                    Price = 2500000,
                    CategoryId = 4,
                    Duration = 60,
                    Level = "Nâng cao",
                    ImageUrl = "/images/docker-k8s.jpg"
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            // Thêm một số UserLog mẫu
            var userLogs = new UserLog[]
            {
                new UserLog
                {
                    Action = "CREATE",
                    EntityName = "Category",
                    EntityId = 1,
                    Description = "Tạo danh mục Lập trình Web",
                    IpAddress = "127.0.0.1",
                    UserName = "Admin",
                    Timestamp = DateTime.Now.AddDays(-1)
                },
                new UserLog
                {
                    Action = "CREATE",
                    EntityName = "Product",
                    EntityId = 1,
                    Description = "Tạo khóa học ASP.NET Core MVC Cơ bản",
                    IpAddress = "127.0.0.1",
                    UserName = "Admin",
                    Timestamp = DateTime.Now.AddHours(-2)
                }
            };

            context.UserLogs.AddRange(userLogs);
            context.SaveChanges();
        }

        private static async Task CreateDefaultUser(UserManager<ApplicationUser> userManager)
        {
            // Kiểm tra xem đã có user admin chưa
            var adminUser = await userManager.FindByEmailAsync("admin@elearn.com");
            if (adminUser == null)
            {
                // Tạo user admin mặc định
                var user = new ApplicationUser
                {
                    UserName = "admin@elearn.com",
                    Email = "admin@elearn.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    Console.WriteLine("Đã tạo user admin mặc định: admin@elearn.com / Admin@123");
                }
            }
        }
    }
}
