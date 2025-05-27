
using EbookMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace EbookMVC.Repository;

public class EFCategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public EFCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Category> GetAll()
    {
        return _context.Categories.ToList();
    }

    public Category GetById(int id)
    {
        var category = _context.Categories.Find(id);
        return category ?? throw new KeyNotFoundException($"Không tìm thấy danh mục có ID: {id}");
    }

    public void Add(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
