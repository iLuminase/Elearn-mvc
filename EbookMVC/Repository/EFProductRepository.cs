using EbookMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EbookMVC.Repository;

public class EFProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public EFProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> GetAll()
    {
        return _context.Products.Include(p => p.Category).ToList();
    }

    public Product GetById(int id)
    {
        return _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id)
            ?? throw new KeyNotFoundException($"Không tìm thấy sản phẩm có ID: {id}");
    }

    public void Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }

    // Tìm kiếm đơn giản chỉ theo từ khóa
    public IEnumerable<Product> Search(string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return new List<Product>();
        }

        var query = _context.Products.Include(p => p.Category).AsQueryable();

        // Tìm kiếm theo tên sản phẩm, mô tả, tên danh mục, hoặc ID
        query = query.Where(p =>
            p.Name.Contains(keyword) ||
            p.Description.Contains(keyword) ||
            p.Category.Name.Contains(keyword) ||
            p.Id.ToString().Contains(keyword) ||
            p.Price.ToString().Contains(keyword));

        return query.ToList();
    }

    //tìm kiếm đa dữ liệu
    public IEnumerable<Product> Search(string keyword, int? categoryId = null,
        decimal? minPrice = null, decimal? maxPrice = null,
        double? minDuration = null, double? maxDuration = null,
        string? level = null)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        //Tim theo tu khoa (ten, mo ta, ten danh muc, id, gia)
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(p =>
                p.Name.Contains(keyword) ||
                p.Description.Contains(keyword) ||
                p.Category.Name.Contains(keyword) ||
                p.Id.ToString().Contains(keyword) ||
                p.Price.ToString().Contains(keyword));
        }

        //Tim theo danh muc
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }
        //Tim theo gia
        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }
        //Tim theo thoi luong
        if (minDuration.HasValue)
        {
            query = query.Where(p => p.Duration >= minDuration.Value);
        }
        if (maxDuration.HasValue)
        {
            query = query.Where(p => p.Duration <= maxDuration.Value);
        }
        //Tim theo cap do
        if (!string.IsNullOrEmpty(level))
        {
            query = query.Where(p => p.Level == level);
        }

        return query.ToList();
    }
}
