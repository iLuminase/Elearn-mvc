using EbookMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EbookMVC.Repository
{
    public class EFUserLogRepository : IUserLogRepository
    {
        private readonly ApplicationDbContext _context;

        public EFUserLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(UserLog log)
        {
            _context.UserLogs.Add(log);
            _context.SaveChanges();
        }

        public IEnumerable<UserLog> GetAll()
        {
            return _context.UserLogs.OrderByDescending(l => l.Timestamp).ToList();
        }

        public IEnumerable<UserLog> GetByAction(string action)
        {
            return _context.UserLogs.Where(l => l.Action == action)
                .OrderByDescending(l => l.Timestamp)
                .ToList();
        }

        public IEnumerable<UserLog> GetByEntityName(string entityName)
        {
            return _context.UserLogs.Where(l => l.EntityName == entityName)
                .OrderByDescending(l => l.Timestamp)
                .ToList();
        }

        public UserLog GetById(int id)
        {
            return _context.UserLogs.Find(id);
        }

        public IEnumerable<UserLog> GetLatest(int count)
        {
            return _context.UserLogs.OrderByDescending(l => l.Timestamp)
                .Take(count)
                .ToList();
        }
    }
}