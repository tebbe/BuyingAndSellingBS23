using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MessageMappingModel
    {
        [Required]
        public string ProductId { get; set; }
        public string MessageId { get; set; }   
        [Required]
        public string SellerId { get; set; } 
        [Required]
        public string BuyerId { get; set; } 
        [Required]
        public string Message { get; set; } 
    }
}
