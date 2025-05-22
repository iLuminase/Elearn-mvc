using Microsoft.AspNetCore.Mvc;
using EbookMVC.Models;
using EbookMVC.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

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
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            // Populate Category property for each product
            foreach (var product in products)
            {
                product.Category = _categoryRepository.GetById(product.CategoryId);
            }
            return View(products);
        }

        // GET: Product/Details/5
        public IActionResult Details(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khóa học!";
                return RedirectToAction(nameof(Index));
            }

            product.Category = _categoryRepository.GetById(product.CategoryId);
            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Add(product);

                // Ghi log
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
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khóa học!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                TempData["ErrorMessage"] = "ID khóa học không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                _productRepository.Update(product);

                // Ghi log
                _userLogRepository.Add(new UserLog
                {
                    Action = "UPDATE",
                    EntityName = "Product",
                    EntityId = product.Id,
                    Description = $"Cập nhật khóa học: {product.Name}",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    UserName = "Admin",
                    Timestamp = DateTime.Now
                });

                // Thêm thông báo thành công
                TempData["SuccessMessage"] = "Cập nhật khóa học thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khóa học!";
                return RedirectToAction(nameof(Index));
            }

            product.Category = _categoryRepository.GetById(product.CategoryId);
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khóa học!";
                return RedirectToAction(nameof(Index));
            }

            string productName = product.Name;
            _productRepository.Delete(id);

            // Ghi log
            _userLogRepository.Add(new UserLog
            {
                Action = "DELETE",
                EntityName = "Product",
                EntityId = id,
                Description = $"Xóa khóa học: {productName}",
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                UserName = "Admin",
                Timestamp = DateTime.Now
            });

            // Thêm thông báo thành công
            TempData["SuccessMessage"] = "Xóa khóa học thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
