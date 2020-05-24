using System;
using System.ComponentModel.DataAnnotations;
namespace NorthwindConsole.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Product Name Required.")]
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }
        [Required(ErrorMessage = "Product must have a unit price.")]
        public decimal? UnitPrice { get; set; }
        public Int16? UnitsInStock { get; set; }
        public Int16? UnitsOnOrder { get; set; }
        public Int16? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        [Required(ErrorMessage = "Product must have a category.")]
        public int? CategoryId { get; set; }
        [Required(ErrorMessage ="Product must have a supplier.")]
        public int? SupplierId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
