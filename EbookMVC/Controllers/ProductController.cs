using Microsoft.AspNetCore.Mvc;
using EbookMVC.Models;
using EbookMVC.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace EbookMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserLogRepository _userLogRepository;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IUserLogRepository userLogRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _userLogRepository = userLogRepository;
        }

        // GET: Product
        // Hiển thị danh sách sản phẩm với tìm kiếm
        public async Task<IActionResult> Index(string searchName, decimal? minPrice, decimal? maxPrice, int? categoryId)
        {
            // Lấy danh sách danh mục để hiển thị trong dropdown
            var categories = await Task.Run(() => _categoryRepository.GetAll());
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            // Sử dụng phương thức Search từ repository để tìm kiếm theo nhiều tiêu chí
            var products = _productRepository.Search(searchName, categoryId, minPrice, maxPrice, null, null, null);

            return View(products.ToList());
        }
        // Hiển thị form thêm sản phẩm mới
        public async Task<IActionResult> Create()
        {
            var categories = await Task.Run(() => _categoryRepository.GetAll());
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        // Xử lý thêm sản phẩm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile imageUrl)
        {
            // Loại bỏ xác thực ModelState cho ImageUrl và Category vì chúng không cần thiết
            ModelState.Remove("ImageUrl");
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageUrl != null)
                    {
                        product.ImageUrl = await SaveImage(imageUrl);
                    }

                    // Lưu sản phẩm vào database
                    _productRepository.Add(product);

                    // Ghi log hoạt động
                    _userLogRepository.Add(new UserLog
                    {
                        Action = "CREATE",
                        EntityName = "Product",
                        EntityId = product.Id,
                        Description = $"Tạo khóa học mới: {product.Name}",
                        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                        UserName = "Admin",
                        Timestamp = DateTime.Now
                    });

                    // Thêm thông báo thành công
                    TempData["SuccessMessage"] = "Thêm khóa học thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Thêm thông báo lỗi
                    TempData["ErrorMessage"] = $"Có lỗi xảy ra khi lưu khóa học: {ex.Message}";
                }
            }

            // Nếu ModelState không hợp lệ hoặc có lỗi, hiển thị form với dữ liệu đã nhập
            var categories = _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        // Viết thêm hàm SaveImage (tham khảo bài 02)
        private async Task<string> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return string.Empty;

            // Tạo tên file unique để tránh trùng lặp
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var uploadsFolder = Path.Combine("wwwroot", "images");

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/images/" + fileName; // Trả về đường dẫn tương đối
        }


        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Details(int id)
        {
            var product = await Task.Run(() => _productRepository.GetById(id));
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Edit(int id)
        {
            var product = await Task.Run(() => _productRepository.GetById(id));
            if (product == null)
            {
                return NotFound();
            }
            var categories = await Task.Run(() => _categoryRepository.GetAll());
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile imageUrl)
        {
            // Loại bỏ xác thực ModelState cho ImageUrl và Category vì chúng không cần thiết
            ModelState.Remove("ImageUrl");
            ModelState.Remove("Category");

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = _productRepository.GetById(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên
                    if (imageUrl == null || imageUrl.Length == 0)
                    {
                        product.ImageUrl = existingProduct.ImageUrl;
                    }
                    else
                    {
                        // Lưu hình ảnh mới
                        product.ImageUrl = await SaveImage(imageUrl);
                    }

                    // Cập nhật các thông tin khác của sản phẩm
                    existingProduct.Name = product.Name;
                    existingProduct.Price = product.Price;
                    existingProduct.Description = product.Description;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.ImageUrl = product.ImageUrl;
                    existingProduct.Duration = product.Duration;
                    existingProduct.Level = product.Level;

                    // Lưu vào database
                    _productRepository.Update(existingProduct);

                    // Ghi log hoạt động
                    _userLogRepository.Add(new UserLog
                    {
                        Action = "UPDATE",
                        EntityName = "Product",
                        EntityId = existingProduct.Id,
                        Description = $"Cập nhật khóa học: {existingProduct.Name}",
                        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                        UserName = "Admin",
                        Timestamp = DateTime.Now
                    });

                    // Thêm thông báo thành công
                    TempData["SuccessMessage"] = "Cập nhật khóa học thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Thêm thông báo lỗi
                    TempData["ErrorMessage"] = $"Có lỗi xảy ra khi cập nhật khóa học: {ex.Message}";
                }
            }

            // Nếu ModelState không hợp lệ hoặc có lỗi, hiển thị form với dữ liệu đã nhập
            var categories = _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var product = await Task.Run(() => _productRepository.GetById(id));
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await Task.Run(() => _productRepository.Delete(id));
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Search với tham số từ URL
        [HttpGet]
        public IActionResult Search(string keyword)
        {
            var viewModel = new ProductSearchViewModel
            {
                Keyword = keyword,
                Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name"),
                Levels = new SelectList(new[] { "Cơ bản", "Trung cấp", "Nâng cao", "Cơ bản đến nâng cao" }),
                IsSearched = !string.IsNullOrEmpty(keyword)
            };

            if (viewModel.IsSearched)
            {
                viewModel.Products = _productRepository.Search(keyword);
            }
            else
            {
                viewModel.Products = new List<Product>();
            }

            return View(viewModel);
        }

        // POST: Product/Search
        [HttpPost]
        public IActionResult Search(ProductSearchViewModel model)
        {
            // Thực hiện tìm kiếm
            var products = _productRepository.Search(
                model.Keyword,
                model.CategoryId,
                model.MinPrice,
                model.MaxPrice,
                null, // minDuration
                null, // maxDuration
                model.Level
            );

            // Cập nhật model
            model.Products = products;
            model.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", model.CategoryId);
            model.Levels = new SelectList(new[] { "Cơ bản", "Trung cấp", "Nâng cao", "Cơ bản đến nâng cao" }, model.Level);
            model.IsSearched = true;

            return View(model);
        }
        // API endpoint cho tìm kiếm real-time
        [HttpGet]
        public IActionResult SearchApi(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return Json(new { success = false, data = new List<object>() });
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

            return Json(new { success = true, data = result });
        }
    }
}
