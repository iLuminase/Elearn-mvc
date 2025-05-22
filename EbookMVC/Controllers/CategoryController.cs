using Microsoft.AspNetCore.Mvc;
using EbookMVC.Models;
using EbookMVC.Repository;
using System;

namespace EbookMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserLogRepository _userLogRepository;
        private readonly IProductRepository _productRepository;

        public CategoryController(
            ICategoryRepository categoryRepository,
            IUserLogRepository userLogRepository,
            IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _userLogRepository = userLogRepository;
            _productRepository = productRepository;
        }

        // GET: Category
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        // GET: Category/Details/5
        public IActionResult Details(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                // Thêm thông báo lỗi
                TempData["ErrorMessage"] = "Không tìm thấy danh mục!";
                return RedirectToAction(nameof(Index));
            }

            // Lấy danh sách sản phẩm thuộc danh mục này
            var products = _productRepository.GetAll().Where(p => p.CategoryId == id).ToList();
            ViewBag.Products = products;

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Add(category);

                // Ghi log
                _userLogRepository.Add(new UserLog
                {
                    Action = "CREATE",
                    EntityName = "Category",
                    EntityId = category.Id,
                    Description = $"Tạo danh mục mới: {category.Name}",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    UserName = "Admin",
                    Timestamp = DateTime.Now
                });

                // Thêm thông báo thành công
                TempData["SuccessMessage"] = "Thêm danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public IActionResult Edit(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                // Thêm thông báo lỗi
                TempData["ErrorMessage"] = "Không tìm thấy danh mục!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                // Thêm thông báo lỗi
                TempData["ErrorMessage"] = "ID danh mục không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                _categoryRepository.Update(category);

                // Ghi log
                _userLogRepository.Add(new UserLog
                {
                    Action = "UPDATE",
                    EntityName = "Category",
                    EntityId = category.Id,
                    Description = $"Cập nhật danh mục: {category.Name}",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    UserName = "Admin",
                    Timestamp = DateTime.Now
                });

                // Thêm thông báo thành công
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                // Thêm thông báo lỗi
                TempData["ErrorMessage"] = "Không tìm thấy danh mục!";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                // Thêm thông báo lỗi
                TempData["ErrorMessage"] = "Không tìm thấy danh mục!";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra xem có sản phẩm nào thuộc danh mục này không
            var hasProducts = _productRepository.GetAll().Any(p => p.CategoryId == id);
            if (hasProducts)
            {
                // Thêm thông báo lỗi
                TempData["ErrorMessage"] = "Không thể xóa danh mục này vì có khóa học đang sử dụng!";
                return RedirectToAction(nameof(Index));
            }

            string categoryName = category.Name;
            _categoryRepository.Delete(id);

            // Ghi log
            _userLogRepository.Add(new UserLog
            {
                Action = "DELETE",
                EntityName = "Category",
                EntityId = id,
                Description = $"Xóa danh mục: {categoryName}",
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                UserName = "Admin",
                Timestamp = DateTime.Now
            });

            // Thêm thông báo thành công
            TempData["SuccessMessage"] = "Xóa danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
