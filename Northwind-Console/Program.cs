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
                    
                    Console.WriteLine("1) Display Categories");
                    Console.WriteLine("2) Add Category");
                    Console.WriteLine("3) Display Category and related products");
                    Console.WriteLine("4) Display all Categories and their related products");
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"Option {choice} selected");
                    if (choice == "1")
                    {
                        var db = new NorthwindContext();
                        var query = db.Categories.OrderBy(p => p.CategoryName);

                        Console.WriteLine($"{query.Count()} records returned");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName} - {item.Description}");
                        }
                    }
                    else if (choice == "2")
                    {

                        Category category = new Category();
                        Console.WriteLine("Enter Category Name:");
                        category.CategoryName = Console.ReadLine();
                        Console.WriteLine("Enter the Category Description:");
                        category.Description = Console.ReadLine();

                        // save category to db
                        var db = new NorthwindContext();




                        try
                        {
                            db.AddCategory(category);
                            logger.Info($"Category Added: {category.CategoryName}");
                        }
                        catch (Exception)
                        {
                            foreach (var validationResult in db.GetValidationErrors())
                            {
                                foreach (var error in validationResult.ValidationErrors)
                                {
                                    logger.Error(
                                        "Entity Property: {0}, Error {1}",
                                        error.PropertyName,
                                        error.ErrorMessage);
                                }
                            }
                        }
                    }

                    else if (choice == "3")
                    {
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
                                Console.WriteLine(p.ProductName);
                            }
                        }
                        catch(NullReferenceException)
                        {
                            logger.Error($"CategoryID invalid.");
                        }
                        

                    }
                    else if (choice == "4")
                    {
                        var db = new NorthwindContext();
                        var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
                        var productList = db.Products.OrderBy(p => p.CategoryId).ToList();
                        List<int?> productCategoryId = new List<int?>();
                        foreach(var productItem in productList)
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

                                    Console.WriteLine($"\t{p.ProductName}");
                                }
                            }
                            
                        }
                    }
                    Console.WriteLine();
                    
                } while (choice.ToLower() != "q");
            }
            
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
