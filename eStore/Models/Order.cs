using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.Models
{
    [Table("Order")]
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Order ID")]
        public int OrderId { get; set; }
        [Required]
        [DisplayName("Member ID")]
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }
        [Required]
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
        [DisplayName("Required Date")]
        public DateTime? RequiredDate { get; set; }
        [DisplayName("Shipped Date")]
        public DateTime? ShippedDate { get; set; }
        [Column(TypeName = "money")]
        public Decimal? Freight { get; set; }
    }
}
