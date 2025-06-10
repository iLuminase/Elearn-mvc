using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EbookMVC.Models;
using EbookMVC.Repository;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using EbookMVC.Areas.Admin.Models;

namespace EbookMVC.Controllers;

[Authorize(Roles = SD.Role_Admin)]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserLogRepository _userLogRepository;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(
        ILogger<HomeController> logger,
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUserLogRepository userLogRepository,
        IEmailSender emailSender,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _userLogRepository = userLogRepository;
        _emailSender = emailSender;
        _userManager = userManager;
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



    // Action test gửi email
    public async Task<IActionResult> TestEmail(string email = "test@example.com")
    {
        try
        {
            await _emailSender.SendEmailAsync(
                email,
                "Test Email từ Elearn System",
                "<h2>Đây là email test</h2><p>Nếu bạn nhận được email này, chức năng gửi email đã hoạt động thành công!</p><p>Thời gian gửi: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</p>");

            TempData["SuccessMessage"] = $"Email test đã được gửi thành công đến {email}";
            _logger.LogInformation("Email test đã được gửi thành công đến {Email}", email);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Lỗi khi gửi email: {ex.Message}";
            _logger.LogError(ex, "Lỗi khi gửi email test đến {Email}", email);
        }

        return RedirectToAction("Index");
    }

    // Action test URL generation cho reset password
    public IActionResult TestResetPasswordUrl()
    {
        try
        {
            // Simulate code generation như trong ForgotPassword
            var code = "test-reset-code-" + DateTime.Now.Ticks;
            var encodedCode = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code = encodedCode },
                protocol: Request.IsHttps ? "https" : "http");

            var emailContent = $"Vui lòng đặt lại mật khẩu của bạn bằng cách <a href='{System.Text.Encodings.Web.HtmlEncoder.Default.Encode(callbackUrl)}'>nhấp vào đây</a>.";

            ViewBag.TestResults = new
            {
                OriginalCode = code,
                EncodedCode = encodedCode,
                CallbackUrl = callbackUrl,
                EmailContent = emailContent,
                Protocol = Request.IsHttps ? "HTTPS" : "HTTP",
                CurrentUrl = Request.Scheme + "://" + Request.Host + Request.Path
            };

            TempData["SuccessMessage"] = "Test URL generation thành công! Kiểm tra kết quả bên dưới.";
            _logger.LogInformation("Test reset password URL: {CallbackUrl}", callbackUrl);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Lỗi khi test URL generation: {ex.Message}";
            _logger.LogError(ex, "Lỗi khi test reset password URL generation");
        }

        // Tạo lại model cho Index view
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

        return View("Index", viewModel);
    }

    // Action để confirm email cho user test
    public async Task<IActionResult> ConfirmUserEmail(string email = "tienphat3968@gmail.com")
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["ErrorMessage"] = $"Không tìm thấy user với email: {email}";
                return RedirectToAction("Index");
            }

            if (user.EmailConfirmed)
            {
                TempData["SuccessMessage"] = $"Email {email} đã được confirm trước đó";
                return RedirectToAction("Index");
            }

            // Manually confirm email
            user.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"Email {email} đã được confirm thành công! Bây giờ có thể test forgot password.";
                _logger.LogInformation("Email confirmed manually for user: {Email}", email);
            }
            else
            {
                TempData["ErrorMessage"] = $"Lỗi khi confirm email: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Lỗi khi confirm email: {ex.Message}";
            _logger.LogError(ex, "Lỗi khi confirm email cho user: {Email}", email);
        }

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
