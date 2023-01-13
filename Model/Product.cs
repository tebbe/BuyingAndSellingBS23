using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Product
    {
        public string Did { get; set; }
        public bool IsUsed { get; set; } = false;
        [Required]
        public string Name { get; set; }
        [Required]
        public string BrandName { get; set; }
        [Required]
        public string Model { get; set; }
        public string Addition { get; set; } = "";
        public string Detail { get; set; } = "";
        public string Contact { get; set; }
        public bool IsActive { get; set; } = true;//for soft delete
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
