using EbookMVC.Models;
using System.Collections.Generic;

namespace EbookMVC.Repository
{
    public interface IUserLogRepository
    {
        IEnumerable<UserLog> GetAll();
        UserLog GetById(int id);
        void Add(UserLog log);
        IEnumerable<UserLog> GetLatest(int count);
        IEnumerable<UserLog> GetByEntityName(string entityName);
        IEnumerable<UserLog> GetByAction(string action);
    }
}
