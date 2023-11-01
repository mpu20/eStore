using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eStore.Models
{
    [Table("Product")]
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Product ID")]
        public int ProductId { get; set; }
        [Required]
        [DisplayName("Category ID")]
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(40)]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(20)]
        public string Weight { get; set; }
        [Required]
        [Column(TypeName = "money")]
        [DisplayName("Unit Price")]
        public Decimal UnitPrice { get; set; }
        [Required]
        [DisplayName("Units In Stock")]
        public int UnitsInStock { get; set; }
    }
}
