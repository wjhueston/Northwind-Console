using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity.Validation;
using NLog;
using NorthwindConsole.Models;

namespace NorthwindConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                string choice;
                do
                {
                    Console.WriteLine("Select a Table to View Options");
                    Console.WriteLine("1) Categories");
                    Console.WriteLine("2) Products");
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    if(choice == "1") 
                    {
                        Console.Clear();
                        logger.Info("Categories Selected");
                        Console.WriteLine("1) Display Categories");
                        Console.WriteLine("2) Add Category");
                        Console.WriteLine("3) Display Category and related products");
                        Console.WriteLine("4) Display all Categories and their related products");
                        Console.WriteLine("5) Edit Category");
                        Console.WriteLine("6) Delete Category");
                        Console.WriteLine("Press any other key to return to the main menu");
                        string categoryChoice = Console.ReadLine();
                        if (categoryChoice == "1")
                        {
                            Console.Clear();
                            logger.Info("Category Option 1 Selected");
                            var db = new NorthwindContext();
                            var query = db.Categories.OrderBy(p => p.CategoryName);

                            Console.WriteLine($"{query.Count()} records returned");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.CategoryName} - {item.Description}");
                            }
                        }
                        else if (categoryChoice == "2")
                        {
                            Console.Clear();
                            logger.Info("Category Option 2 Selected");
                            Category category = new Category();
                            Console.WriteLine("Enter Category Name:");
                            category.CategoryName = Console.ReadLine();
                            Console.WriteLine("Enter the Category Description:");
                            category.Description = Console.ReadLine();
                            var db = new NorthwindContext();
                            db.Categories.Add(category);

                            if (GetValidation(db)) 
                            {
                                logger.Info($"Category Added: {category.CategoryName}");
                                db.SaveChanges();
                            }
                        }

                        else if (categoryChoice == "3")
                        {
                            Console.Clear();
                            logger.Info("Category Option 3 Selected");
                            var db = new NorthwindContext();
                            var query = db.Categories.OrderBy(p => p.CategoryId);
                            List<int> categoryIdCheck = new List<int>();

                            Console.WriteLine("Select the category whose products you want to display:");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                                categoryIdCheck.Add(item.CategoryId);
                            }
                            int id = int.Parse(Console.ReadLine());
                            Console.Clear();
                            logger.Info($"CategoryId {id} selected");

                            try
                            {
                                Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
                                Console.WriteLine($"{category.CategoryName} - {category.Description}");
                                foreach (Product p in category.Products)
                                {
                                    if (p.Discontinued == false)
                                    {
                                        Console.WriteLine(p.ProductName);
                                    }
                                }
                            }
                            catch (NullReferenceException)
                            {
                                logger.Error($"CategoryID invalid.");
                            }


                        }
                        else if (categoryChoice == "4")
                        {
                            Console.Clear();
                            logger.Info("Category Option 4 Selected");
                            var db = new NorthwindContext();
                            var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
                            var productList = db.Products.OrderBy(p => p.CategoryId).ToList();
                            List<int?> productCategoryId = new List<int?>();
                            foreach (var productItem in productList)
                            {
                                productCategoryId.Add(productItem.CategoryId);
                            }
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.CategoryName}");
                                if (!productCategoryId.Contains(item.CategoryId))
                                {
                                    Console.WriteLine("\tNo products found.");
                                }
                                else
                                {
                                    foreach (Product p in item.Products)
                                    {
                                        if (p.Discontinued == false)
                                        {
                                            Console.WriteLine($"\t{p.ProductName}");
                                        }
                                    }
                                }

                            }
                        }
                        else if (categoryChoice == "5")
                        {
                            Console.Clear();
                            logger.Info("Category Option 5 Selected");
                            var db = new NorthwindContext();
                            var query = db.Categories.OrderBy(c => c.CategoryId);
                            Console.WriteLine("Select the category to edit:");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.CategoryId}. {item.CategoryName}");
                            }
                            
                            string select = Console.ReadLine();
                            foreach(var item in query)
                            {
                                if(item.CategoryId == Convert.ToInt32(select))
                                {
                                    Console.WriteLine($"Enter the new Category Name. Was: {item.CategoryName}");
                                    string name = Console.ReadLine();
                                    Console.WriteLine($"Enter the new Description. Was: {item.Description}");
                                    string description = Console.ReadLine();
                                    item.CategoryName = name;
                                    item.Description = description;
                                    if (GetValidation(db))
                                    {
                                        logger.Info($"Category Updated: {name}");
                                        db.SaveChanges();
                                    }
                                }
                            }
                            
                        
                            
                            
                            
                        }
                        else if (categoryChoice == "6")
                        {
                            Console.Clear();
                            logger.Info("Category Option 6 Selected");
                            var db = new NorthwindContext();
                            using (db)
                            {
                                var query = db.Categories.OrderBy(c => c.CategoryId);
                                Console.WriteLine("Select the category to be deleted. Note that products in this category will also be removed.");
                                foreach (var item in query)
                                {
                                    Console.WriteLine($"{item.CategoryId}. {item.CategoryName}");
                                }
                                try
                                {
                                    int select = Convert.ToInt32(Console.ReadLine());
                                    foreach (var item in query)
                                    {
                                        if (item.CategoryId == select)
                                        {
                                            db.Categories.Remove(item);
                                        }
                                    }
                                    var productQuery = db.Products.OrderBy(p => p.ProductID);
                                    foreach (var item in productQuery)
                                    {
                                        if (item.CategoryId == select)
                                        {
                                            db.Products.Remove(item);
                                        }
                                    }

                                }
                                catch (Exception e)
                                {
                                    if (e.InnerException is FormatException)
                                    {
                                        logger.Error("Invalid Selection.");
                                    }
                                }
                                db.SaveChanges();
                            }
                        }
                    }
                    else if(choice == "2") 
                    {
                        Console.Clear();
                        logger.Info("Products Selected");
                        Console.WriteLine("1) Add Product");
                        Console.WriteLine("2) Edit Product");
                        Console.WriteLine("3) Display all Products");
                        Console.WriteLine("4) Display Product Details");
                        Console.WriteLine("5) Delete Product");
                        Console.WriteLine("Press any other key to return to the main menu");
                        string productChoice = Console.ReadLine();
                        if (productChoice == "1")
                        {
                            Console.Clear();
                            logger.Info("Product Option 1 Selected");
                            var db = new NorthwindContext();
                            
                            Product product = new Product();
                            Console.WriteLine("Enter Product Name");
                            string name = Console.ReadLine();
                            Console.WriteLine("Enter product price");
                            string unitPrice = Console.ReadLine();
                            Console.WriteLine("Select the category for this product");
                            var categoryQuery = db.Categories.OrderBy(c => c.CategoryId);
                            foreach(var item in categoryQuery)
                            {
                                Console.WriteLine($"{item.CategoryId}. {item.CategoryName}");
                            }
                            string categorySelect = Console.ReadLine();
                            try
                            {
                                
                                foreach (var item in categoryQuery)
                                {
                                    if (Convert.ToInt32(categorySelect) == item.CategoryId)
                                    {
                                        product.CategoryId = item.CategoryId;
                                    }
                                }
                            }
                            catch
                            {
                                logger.Error("Invalid Category Selection");
                                continue;
                            }
                            
                            Console.WriteLine("Select the supplier for this product");
                            var supplierQuery = db.Suppliers.OrderBy(s => s.SupplierId);
                            foreach(var item in supplierQuery)
                            {
                                Console.WriteLine($"{item.SupplierId}. {item.CompanyName}");
                            }
                            string supplierSelect = Console.ReadLine();
                            try
                            {
                                foreach (var item in supplierQuery)
                                {
                                    if (Convert.ToInt32(supplierSelect) == item.SupplierId)
                                    {
                                        product.SupplierId = item.SupplierId;
                                    }
                                }
                            }
                            catch
                            {
                                logger.Error("Invalid Supplier Selection");
                                continue;
                            }
                            product.ProductName = name;
                            try
                            {
                                product.UnitPrice = Convert.ToDecimal(unitPrice);
                            }
                            catch
                            {
                                logger.Error("Invalid Selection.");
                                continue;
                            }
                            db.Products.Add(product);
                            if (GetValidation(db))
                            {
                                logger.Info($"Product Added: {product.ProductName}");
                                db.SaveChanges();
                            }                   
                        }
                        else if (productChoice == "2")
                        {
                            Console.Clear();
                            logger.Info("Product Option 2 Selected");
                            var db = new NorthwindContext();
                            using (db)
                            {
                                var query = db.Products.OrderBy(p => p.ProductID);

                                Console.WriteLine("Select a product to edit:");
                                foreach (var item in query)
                                {
                                    Console.WriteLine($"{item.ProductID}. {item.ProductName}");
                                }
                                string editSelection = Console.ReadLine();
                                try
                                {
                                    /////////
                                    /*
                                    foreach (var item in query)
                                    {
                                        if (item.ProductID == Convert.ToInt32(editSelection))
                                        {
                                            
                                            //Currently just lets the user update name and price
                                            Console.WriteLine($"Enter the new product name. Was: {item.ProductName}");
                                            string name = Console.ReadLine();
                                            Console.WriteLine($"Enter the new price. Was: ${item.UnitPrice}");
                                            string price = Console.ReadLine();
                                            try
                                            {
                                                item.UnitPrice = Convert.ToDecimal(price);
                                            }
                                            catch
                                            {
                                                logger.Error("Invalid Price");
                                                continue;
                                            }
                                            item.ProductName = name;
                                            if (GetValidation(db))
                                            {
                                                logger.Info($"Product Updated: {name}");
                                                db.SaveChanges();
                                            }
                                        }
                                            
                                            
                                    }
                                    */
                                    /////////
                                    Console.Clear();
                                    int productSelect;
                                    try
                                    {
                                        productSelect = Convert.ToInt32(editSelection);
                                    }
                                    catch
                                    {
                                        logger.Error("Invalid Selection");
                                        continue;
                                    }
                                    var productQuery = db.Products.FirstOrDefault(p => p.ProductID == productSelect);
                                    Console.WriteLine("Select the attribute to edit:");
                                    Console.WriteLine($"1) Product Name : {productQuery.ProductName}");
                                    Console.WriteLine($"2) Unit Price: {productQuery.UnitPrice}");
                                    Console.WriteLine($"3) Quantity Per Unit: {productQuery.QuantityPerUnit}");
                                    Console.WriteLine($"4) Units in Stock: {productQuery.UnitsInStock}");
                                    Console.WriteLine($"5) Units on Order: {productQuery.UnitsOnOrder}");
                                    Console.WriteLine($"6) Reorder Level: {productQuery.ReorderLevel}");
                                    if (productQuery.Discontinued == true)
                                    {
                                        Console.WriteLine($"7) Discontinued: Yes");
                                    }
                                    else
                                    {
                                        Console.WriteLine("7) Discontinued: No");
                                    }
                                    Console.WriteLine($"8) Category Name: {productQuery.Category.CategoryName}");
                                    Console.WriteLine($"9) Supplier Name: {productQuery.Supplier.CompanyName}");
                                    string submenu = Console.ReadLine();
                                    switch (submenu)
                                    {
                                        case "1":
                                            logger.Info("Product Edit Option 1 Selected");
                                            Console.WriteLine($"Enter the new product name. Was: {productQuery.ProductName}");
                                            string name = Console.ReadLine();
                                            productQuery.ProductName = name;
                                            if (GetValidation(db))
                                            {
                                                logger.Info($"Product Name Updated: {name}");
                                                db.SaveChanges();
                                                break;
                                            }
                                            break;
                                        case "2":
                                            logger.Info("Product Edit Option 2 Selected");
                                            Console.WriteLine($"Enter the new price. Was: ${productQuery.UnitPrice}");
                                            string price = Console.ReadLine();
                                            try
                                            {
                                                productQuery.UnitPrice = Convert.ToDecimal(price);
                                                if (GetValidation(db))
                                                {
                                                    logger.Info($"Product Price Updated: ${Convert.ToDecimal(price)}");
                                                    db.SaveChanges();
                                                    break;
                                                }
                                            }
                                            catch
                                            {
                                                logger.Error("Invalid Price");
                                                break;
                                            }
                                            break;
                                        case "3":
                                            logger.Info("Product Edit Option 3 Selected");
                                            if (productQuery.QuantityPerUnit == null)
                                            {
                                                Console.WriteLine("Enter the quantity per unit.");
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Enter the quantity per unit. Was: {productQuery.QuantityPerUnit}");
                                            }
                                            string quantityPerUnit = Console.ReadLine();
                                            productQuery.QuantityPerUnit = quantityPerUnit;
                                            break;
                                        case "4":
                                            logger.Info("Product Edit Option 4 Selected");
                                            if (productQuery.UnitsInStock == null)
                                            {
                                                Console.WriteLine("Enter the units in stock.");
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Enter the units in stock. Was: {productQuery.UnitsInStock}");
                                            }
                                            string unitsInStock = Console.ReadLine();
                                            try
                                            {
                                                productQuery.UnitsInStock = Convert.ToInt16(unitsInStock);
                                            }
                                            catch
                                            {
                                                logger.Error("Invalid Entry");
                                                break;
                                            }
                                            break;
                                        case "5":
                                            logger.Info("Product Edit Option 5 Selected");
                                            if (productQuery.UnitsOnOrder == null)
                                            {
                                                Console.WriteLine("Enter the units on order.");
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Enter the units on order. Was: {productQuery.UnitsOnOrder}");
                                            }
                                            string unitsOnOrder = Console.ReadLine();
                                            try
                                            {
                                                productQuery.UnitsOnOrder = Convert.ToInt16(unitsOnOrder);
                                            }
                                            catch
                                            {
                                                logger.Error("Invalid Entry");
                                                break;
                                            }
                                            break;
                                        case "6":
                                            logger.Info("Product Edit Option 6 Selected");
                                            if (productQuery.ReorderLevel == null)
                                            {
                                                Console.WriteLine("Enter the reorder level.");
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Enter the reorder level. Was: {productQuery.ReorderLevel}");
                                            }
                                            string reorderLevel = Console.ReadLine();
                                            try
                                            {
                                                productQuery.ReorderLevel = Convert.ToInt16(reorderLevel);
                                            }
                                            catch
                                            {
                                                logger.Error("Invalid Entry");
                                                break;
                                            }
                                            break;
                                        case "7":
                                            logger.Info("Product Edit Option 7 Selected");
                                            Console.WriteLine($"Enter the discontinued status. Was: {productQuery.Discontinued}");
                                            string discontinued = Console.ReadLine();
                                            try
                                            {
                                                productQuery.Discontinued = Convert.ToBoolean(discontinued);
                                            }
                                            catch
                                            {
                                                logger.Error("Invalid Entry");
                                                break;
                                            }
                                            break;
                                        case "8":
                                            logger.Info("Product Edit Option 8 Selected");
                                            Console.WriteLine($"Select the category for this product. Was: {productQuery.Category.CategoryName}");
                                            var categoryQuery = db.Categories.OrderBy(c => c.CategoryId).ToList();
                                            foreach (var item in categoryQuery)
                                            {
                                                Console.WriteLine($"{item.CategoryId}. {item.CategoryName}");
                                            }
                                            string categorySelect = Console.ReadLine();
                                            try
                                            {

                                                foreach (var item in categoryQuery)
                                                {
                                                    if (Convert.ToInt32(categorySelect) == item.CategoryId)
                                                    {
                                                        productQuery.CategoryId = item.CategoryId;
                                                        if (GetValidation(db))
                                                        {
                                                            logger.Info($"Product Category Updated: {productQuery.Category.CategoryName}");
                                                            db.SaveChanges();
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                logger.Error("Invalid Category Selection");
                                                break;
                                            }
                                            break;
                                        case "9":
                                            logger.Info("Product Edit Option 9 Selected");
                                            Console.WriteLine($"Select the supplier for this product. Was:{productQuery.Supplier.CompanyName}");
                                            var supplierQuery = db.Suppliers.OrderBy(s => s.SupplierId).ToList();
                                            foreach (var item in supplierQuery)
                                            {
                                                Console.WriteLine($"{item.SupplierId}. {item.CompanyName}");
                                            }
                                            string supplierSelect = Console.ReadLine();
                                            try
                                            {
                                                foreach (var item in supplierQuery)
                                                {
                                                    if (Convert.ToInt32(supplierSelect) == item.SupplierId)
                                                    {
                                                        productQuery.SupplierId = item.SupplierId;
                                                        if (GetValidation(db))
                                                        {
                                                            logger.Info($"Product Supplier Updated: {productQuery.Supplier.CompanyName}");
                                                            db.SaveChanges();
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                logger.Error("Invalid Supplier Selection");
                                                continue;
                                            }
                                            break;
                                        default:
                                            logger.Error("Invalid Selection");
                                            break;
                                    }
                                }

                                catch
                                {
                                    logger.Error("Invalid Selection");
                                    continue;
                                }
                                
                            }
                        }
                        else if (productChoice == "3")
                        {
                            Console.Clear();
                            logger.Info("Product Option 3 Selected");
                            var db = new NorthwindContext();
                            using (db)
                            {
                                Console.WriteLine("Display products of what type?");
                                Console.WriteLine("1) Active");
                                Console.WriteLine("2) Discontinued");
                                Console.WriteLine("3) All Products");
                                string submenu = Console.ReadLine();
                                var query = db.Products.OrderBy(p => p.ProductName);
                                switch (submenu)
                                {
                                    case "1":
                                        Console.Clear();
                                        logger.Info("Inner Option 1 Selected");
                                        Console.WriteLine("All Active Products");
                                        foreach (var item in query)
                                        {
                                            if (item.Discontinued == false)
                                            {
                                                Console.WriteLine($"{item.ProductName}");
                                            }
                                        }
                                        break;
                                    case "2":
                                        Console.Clear();
                                        logger.Info("Inner Option 2 Selected");
                                        Console.WriteLine("All Discontinued Products");
                                        foreach (var item in query)
                                        {
                                            if (item.Discontinued == true)
                                            {
                                                Console.WriteLine($"{item.ProductName}");
                                            }
                                        }
                                        break;
                                    case "3":
                                        Console.Clear();
                                        logger.Info("Inner Option 3 Selected");
                                        Console.WriteLine("All Products");
                                        foreach (var item in query)
                                        {
                                            Console.WriteLine($"{item.ProductName}");
                                        }
                                        break;
                                    default:
                                        logger.Error("Invalid selection.");
                                        break;
                                }
                            }
                        }
                        else if (productChoice == "4")
                        {
                            Console.Clear();
                            logger.Info("Product Option 4 Selected");
                            var db = new NorthwindContext();
                            using (db)
                            {
                                var query = db.Products.OrderBy(p => p.ProductID);
                                Console.WriteLine("Select the product to view details");
                                foreach (var item in query)
                                {
                                    Console.WriteLine($"{item.ProductID}. {item.ProductName}");
                                }
                                try
                                {
                                    int select = Convert.ToInt32(Console.ReadLine());
                                    Product product = db.Products.FirstOrDefault(p => p.ProductID == select);
                                    if (product == null)
                                    {
                                        logger.Error("Product selection not found.");
                                        continue;
                                    }
                                    Console.Clear();
                                    Console.WriteLine($"Product ID: {product.ProductID}");
                                    Console.WriteLine($"Product Name : {product.ProductName}");
                                    Console.WriteLine($"Unit Price: {product.UnitPrice}");
                                    Console.WriteLine($"Quantity Per Unit: {product.QuantityPerUnit}");
                                    Console.WriteLine($"Units in Stock: {product.UnitsInStock}");
                                    Console.WriteLine($"Units on Order: {product.UnitsOnOrder}");
                                    Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
                                    if (product.Discontinued == true)
                                    {
                                        Console.WriteLine($"Discontinued: Yes");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Discontinued: No");
                                    }
                                    Console.WriteLine($"Category Name: {product.Category.CategoryName}");
                                    Console.WriteLine($"Supplier Name: {product.Supplier.CompanyName}");


                                }
                                catch (Exception e)
                                {
                                    if (e.InnerException is FormatException)
                                    {
                                        logger.Error("Invalid selection.");
                                    }
                                }
                            }
                        }

                        else if (productChoice == "5")
                        {
                            Console.Clear();
                            logger.Info("Product Option 5 Selected");
                            var db = new NorthwindContext();
                            using (db)
                            {
                                var query = db.Products.OrderBy(p => p.ProductID);
                                Console.WriteLine("Select the product to be deleted.");
                                foreach (var item in query)
                                {
                                    Console.WriteLine($"{item.ProductID}. {item.ProductName}");
                                }
                                try
                                {
                                    int select = Convert.ToInt32(Console.ReadLine());

                                    foreach (var item in query)
                                    {
                                        if (item.ProductID == select)
                                        {
                                            db.Products.Remove(item);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    if (e.InnerException is FormatException)
                                    {
                                        logger.Error("Invalid Selection.");
                                    }
                                    else
                                    {
                                        GetValidation(db);
                                    }
                                }
                                db.SaveChanges();
                            }
                        }
                    }
                    ///////
                    
                    
                    
                    
                    Console.WriteLine();
                    
                } while (choice.ToLower() != "q");
            }
            catch (DbEntityValidationException e)
            {
                logger.Error(e.Message);
                foreach (var eve in e.EntityValidationErrors)
                {
                    logger.Error("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        logger.Error("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
        static bool GetValidation(NorthwindContext db)
        {
            foreach (var validationResult in db.GetValidationErrors())
            {
                foreach (var error in validationResult.ValidationErrors)
                {
                    logger.Error(
                        "Entity Property: {0}, Error {1}",
                        error.PropertyName,
                        error.ErrorMessage);
                    return false;
                }
                
            }
            return true;
        }
    }
}
