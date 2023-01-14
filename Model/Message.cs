using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Message
    {
        public string Did { get; set; }
        [Required]
        [DisplayName("Product ID")]
        public string ProductId { get; set; }
        [Required]
        [DisplayName("Seller ID")]
        public string SellerId { get; set; }
        [Required]
        [DisplayName("Buyer ID")]
        public string BuyerId { get; set; }
        public bool IsBlockBuyer { get; set; } = false;//if "true" buyer will be block
        public DateTime InsertedDate { get; set; }
    }
}
