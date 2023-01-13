using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductTag
    {
        public string Did { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string TagName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
