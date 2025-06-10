using Microsoft.AspNetCore.Mvc;
using EbookMVC.Models;
using EbookMVC.Repository;
using Microsoft.AspNetCore.Authorization;
using EbookMVC.Areas.Admin.Models;
using System;
using System.Linq;

namespace EbookMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
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

        // GET: Admin/Category
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        // GET: Admin/Category/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID danh mục không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            var category = _categoryRepository.GetById(id.Value);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy danh mục!";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
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
                    UserName = User.Identity?.Name ?? "Admin",
                    Timestamp = DateTime.Now
                });

                TempData["SuccessMessage"] = "Thêm danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID danh mục không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            var category = _categoryRepository.GetById(id.Value);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy danh mục!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.Id)
            {
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
                    UserName = User.Identity?.Name ?? "Admin",
                    Timestamp = DateTime.Now
                });

                TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID danh mục không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            var category = _categoryRepository.GetById(id.Value);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy danh mục!";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category != null)
            {
                // Kiểm tra xem có sản phẩm nào đang sử dụng danh mục này không
                var productsInCategory = _productRepository.GetAll().Where(p => p.CategoryId == id).ToList();
                if (productsInCategory.Any())
                {
                    TempData["ErrorMessage"] = $"Không thể xóa danh mục '{category.Name}' vì còn {productsInCategory.Count} khóa học đang sử dụng!";
                    return RedirectToAction(nameof(Index));
                }

                _categoryRepository.Delete(id);

                // Ghi log
                _userLogRepository.Add(new UserLog
                {
                    Action = "DELETE",
                    EntityName = "Category",
                    EntityId = id,
                    Description = $"Xóa danh mục: {category.Name}",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    UserName = User.Identity?.Name ?? "Admin",
                    Timestamp = DateTime.Now
                });

                TempData["SuccessMessage"] = "Xóa danh mục thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy danh mục để xóa!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
