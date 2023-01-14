using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.QueryString
{
    public class MessageQueryModel
    {
        [Required]
        public string ProductId { get; set; }//selected product post
        [Required]
        public string SellerId { get; set; }//login seller
        [Required]
        public string BuyerId { get; set; }//login buyer
    }
}
