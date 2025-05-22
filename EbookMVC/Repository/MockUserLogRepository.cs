using EbookMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EbookMVC.Repository
{
    public class MockUserLogRepository : IUserLogRepository
    {
        private List<UserLog> _logs = new List<UserLog>
        {
            new UserLog
            {
                Id = 1,
                Action = "CREATE",
                EntityName = "Product",
                EntityId = 1,
                Description = "Tạo khóa học mới: Lập trình C# cơ bản đến nâng cao",
                IpAddress = "127.0.0.1",
                UserName = "Admin",
                Timestamp = DateTime.Now.AddDays(-5)
            },
            new UserLog
            {
                Id = 2,
                Action = "UPDATE",
                EntityName = "Product",
                EntityId = 1,
                Description = "Cập nhật khóa học: Lập trình C# cơ bản đến nâng cao",
                IpAddress = "127.0.0.1",
                UserName = "Admin",
                Timestamp = DateTime.Now.AddDays(-3)
            },
            new UserLog
            {
                Id = 3,
                Action = "CREATE",
                EntityName = "Product",
                EntityId = 2,
                Description = "Tạo khóa học mới: ASP.NET Core MVC",
                IpAddress = "127.0.0.1",
                UserName = "Admin",
                Timestamp = DateTime.Now.AddDays(-2)
            },
            new UserLog
            {
                Id = 4,
                Action = "CREATE",
                EntityName = "Category",
                EntityId = 1,
                Description = "Tạo danh mục mới: Lập trình Backend",
                IpAddress = "127.0.0.1",
                UserName = "Admin",
                Timestamp = DateTime.Now.AddDays(-7)
            },
            new UserLog
            {
                Id = 5,
                Action = "DELETE",
                EntityName = "Product",
                EntityId = 5,
                Description = "Xóa khóa học: Python cho người mới bắt đầu",
                IpAddress = "127.0.0.1",
                UserName = "Admin",
                Timestamp = DateTime.Now.AddHours(-12)
            }
        };

        public IEnumerable<UserLog> GetAll()
        {
            return _logs.OrderByDescending(l => l.Timestamp);
        }

        public UserLog GetById(int id)
        {
            return _logs.FirstOrDefault(l => l.Id == id);
        }

        public void Add(UserLog log)
        {
            log.Id = _logs.Count > 0 ? _logs.Max(l => l.Id) + 1 : 1;
            _logs.Add(log);
        }

        public IEnumerable<UserLog> GetLatest(int count)
        {
            return _logs.OrderByDescending(l => l.Timestamp).Take(count);
        }

        public IEnumerable<UserLog> GetByEntityName(string entityName)
        {
            return _logs.Where(l => l.EntityName.Equals(entityName, StringComparison.OrdinalIgnoreCase))
                        .OrderByDescending(l => l.Timestamp);
        }

        public IEnumerable<UserLog> GetByAction(string action)
        {
            return _logs.Where(l => l.Action.Equals(action, StringComparison.OrdinalIgnoreCase))
                        .OrderByDescending(l => l.Timestamp);
        }
    }
}
