using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchShop.DataAccess.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Телефон обязателен")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Адрес обязателен")]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}