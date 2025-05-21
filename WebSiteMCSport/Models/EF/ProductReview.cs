using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebSiteMCSport.Models.EF;

namespace BanHangThoiTrangMVC.Models.EF
{
    [Table("tb_ProductReview")]
    public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }

        [Range(1, 5)]
        [Required]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual Product Product { get; set; }
    }
}