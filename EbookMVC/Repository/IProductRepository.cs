using EbookMVC.Models;
using System.Collections.Generic;

namespace EbookMVC.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);

        //Cập nhật chức năng tìm kiếm đa dữ liệu
        IEnumerable<Product> Search(string keyword);
        IEnumerable<Product> Search(string keyword, int? categoryId = null,
        decimal? minPrice = null, decimal? maxPrice = null,
        double? minDuration = null, double? maxDuration = null,
        string? level = null);
    }
}
