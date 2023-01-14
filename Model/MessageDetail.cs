using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MessageDetail
    {
        public string  Did { get; set; }
        [Required]
        public string  MessageId { get; set; }
        public DateTime InsertedDate { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
