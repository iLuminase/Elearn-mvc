using System;
using System.ComponentModel.DataAnnotations;

namespace EbookMVC.Models
{
    public class UserLog
    {
        public int Id { get; set; }

        [Display(Name = "Hành động")]
        public string Action { get; set; } = string.Empty;

        [Display(Name = "Đối tượng")]
        public string EntityName { get; set; } = string.Empty;

        [Display(Name = "ID đối tượng")]
        public int? EntityId { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "IP")]
        public string IpAddress { get; set; } = string.Empty;

        [Display(Name = "Người dùng")]
        public string UserName { get; set; } = "Admin";

        [Display(Name = "Thời gian")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
