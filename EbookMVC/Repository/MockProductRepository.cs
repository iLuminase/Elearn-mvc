using EbookMVC.Models;
using System.Collections.Generic;

namespace EbookMVC.Repository
{
    public class MockProductRepository : IProductRepository
    {
        private List<Product> _products = new List<Product>
        {
            new Product {
                Id = 1,
                Name = "Lập trình C# cơ bản đến nâng cao",
                Price = 1290000m,
                Description = "Khóa học lập trình C# từ cơ bản đến nâng cao, phù hợp cho người mới bắt đầu",
                CategoryId = 1,
                Duration = 40,
                Level = "Cơ bản đến nâng cao"
            },
            new Product {
                Id = 2,
                Name = "ASP.NET Core MVC",
                Price = 1350000m,
                Description = "Xây dựng ứng dụng web với ASP.NET Core MVC và Entity Framework Core",
                CategoryId = 2,
                Duration = 35,
                Level = "Trung cấp"
            },
            new Product {
                Id = 3,
                Name = "SQL Server cho người mới bắt đầu",
                Price = 850000m,
                Description = "Học cách thiết kế, quản lý và tối ưu cơ sở dữ liệu SQL Server",
                CategoryId = 3,
                Duration = 25,
                Level = "Cơ bản"
            },
            new Product {
                Id = 4,
                Name = "JavaScript & React.js",
                Price = 1220000m,
                Description = "Phát triển ứng dụng web hiện đại với JavaScript và React.js",
                CategoryId = 4,
                Duration = 30,
                Level = "Trung cấp"
            }
        };

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Duration = product.Duration;
                existingProduct.Level = product.Level;
            }
        }

        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
