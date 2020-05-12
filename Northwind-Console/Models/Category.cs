using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthwindConsole.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Category Name required.")]
        public string CategoryName { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
