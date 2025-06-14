using Microsoft.AspNetCore.Mvc;
using EbookMVC.Models;
using EbookMVC.Repository;
using Microsoft.AspNetCore.Authorization;
using EbookMVC.Areas.Admin.Models;
using System.Linq;
using System.Collections.Generic;

namespace EbookMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserLogRepository _userLogRepository;

        public HomeController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IUserLogRepository userLogRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _userLogRepository = userLogRepository;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll().ToList();
            var categories = _categoryRepository.GetAll().ToList();
            var recentLogs = _userLogRepository.GetLatest(10).ToList();

            // Tính tổng doanh thu (giả định)
            decimal totalRevenue = products.Sum(p => p.Price * 10); // Giả sử mỗi khóa học bán được 10 bản

            // Thống kê số lượng sản phẩm theo danh mục
            var productsByCategory = new Dictionary<string, int>();
            foreach (var category in categories)
            {
                int count = products.Count(p => p.CategoryId == category.Id);
                productsByCategory.Add(category.Name, count);
            }

            // Thống kê số lượng hành động theo loại
            var actionCounts = _userLogRepository.GetAll()
                .GroupBy(l => l.Action)
                .ToDictionary(g => g.Key, g => g.Count());

            var viewModel = new DashboardViewModel
            {
                TotalProducts = products.Count,
                TotalCategories = categories.Count,
                TotalRevenue = totalRevenue,
                TotalUsers = 50, // Giả định có 50 người dùng
                RecentActivities = recentLogs,
                ProductsByCategory = productsByCategory,
                ActionCounts = actionCounts
            };

            return View(viewModel);
        }

        public IActionResult UserLogs()
        {
            var logs = _userLogRepository.GetAll();
            return View(logs);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
