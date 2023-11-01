using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eStore.Models
{
    [Table("OrderDetail")]
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderDetail
    {
        [DisplayName("Order ID")]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        [DisplayName("Product ID")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [Required]
        [Column(TypeName = "money")]
        [DisplayName("Unit Price")]
        public Decimal UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float Discount { get; set; }
    }
}
