using EbookMVC.Models;
using System.Collections.Generic;

namespace EbookMVC.Repository
{
    public class MockCategoryRepository : ICategoryRepository
    {
        private List<Category> _categories = new List<Category>
        {
            new Category { Id = 1, Name = "Lập trình Backend", Description = "Các khóa học về lập trình phía máy chủ" },
            new Category { Id = 2, Name = "Lập trình Web", Description = "Các khóa học về phát triển ứng dụng web" },
            new Category { Id = 3, Name = "Cơ sở dữ liệu", Description = "Các khóa học về quản trị và thiết kế cơ sở dữ liệu" },
            new Category { Id = 4, Name = "Lập trình Frontend", Description = "Các khóa học về phát triển giao diện người dùng" }
        };

        public IEnumerable<Category> GetAll()
        {
            return _categories;
        }

        public Category GetById(int id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Category category)
        {
            category.Id = _categories.Max(c => c.Id) + 1;
            _categories.Add(category);
        }

        public void Update(Category category)
        {
            var existingCategory = _categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
            }
        }

        public void Delete(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _categories.Remove(category);
            }
        }
    }
}
