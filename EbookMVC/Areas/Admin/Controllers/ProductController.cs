﻿using EbookMVC.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EbookMVC.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace EbookMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(string searchName, decimal? minPrice, decimal? maxPrice, int? categoryId)
        {
            // Lấy danh sách danh mục để hiển thị trong dropdown
            var categories = await Task.Run(() => _categoryRepository.GetAll());
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            // Sử dụng phương thức Search từ repository để tìm kiếm theo nhiều tiêu chí
            var products = _productRepository.Search(searchName, categoryId, minPrice, maxPrice, null, null, null);

            return View(products.ToList());
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

        // GET: Admin/Product/Search với tham số từ URL
        [HttpGet]
        public IActionResult Search(string keyword)
        {
            // Redirect về Index với keyword
            return RedirectToAction("Index", new { searchName = keyword });
        }

        // GET: Admin/Product/Details/5
        public IActionResult Details(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khóa học!";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            var categories = _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // GET: Admin/Product/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khóa học!";
                return RedirectToAction(nameof(Index));
            }
            var categories = _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khóa học!";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    }
}
