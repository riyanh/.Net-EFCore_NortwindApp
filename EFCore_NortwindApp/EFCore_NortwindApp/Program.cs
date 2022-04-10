using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using static System.Console;
using EFCore_NortwindDb;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore_NortwindApp
{
    class program
    {
        private static IConfigurationRoot Configuration;

        private static DbContextOptionsBuilder<NorthwindContext> optionBuilder;
        static void Main(string[] args)
        {
            BuildConfiguration();
            Console.WriteLine("Connection String : " + Configuration.GetConnectionString("NorthwindDS"));
            BuildOptions();
            //ListCustomer();

            //insertCategory();
            //updateCategory();
            //DeleteCategory();
            //FilteringAndSorting();
            SelectFilterSorting();
            //AgregateProduct();
            //GetProductSupplierDto();
        }

        static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        static void BuildOptions()
        {
            optionBuilder = new DbContextOptionsBuilder<NorthwindContext>();
            optionBuilder.UseSqlServer(Configuration.GetConnectionString("NorthwindDS"));
        }

        static void ListCustomer()
        {
            using (var db = new NorthwindContext(optionBuilder.Options))
            {
                var customers = db.Customers.OrderByDescending(x => x.CompanyName).Take(10).ToList();

                foreach (var cust in customers)
                {
                    Console.WriteLine($"{cust.CompanyName} {cust.ContactName}");
                }
            } 
        }

        //insert Category
        static void insertCategory()
        {
            using( var db = new NorthwindContext(optionBuilder.Options))
            {
                var isCategoryExist = db.Categories.Find(9);
                if(isCategoryExist == null)
                {
                    var category = new Category()
                    {
                        CategoryName = "Korean food",
                        Description = "Soju Mint"
                    };
                    db.Categories.Add(category);
                    db.SaveChanges();
                    Console.WriteLine("Insert data is successed");
                }
                else
                {
                    Console.WriteLine($"Sorry Your data is exist");
                }

            }
        }//endInserCategory

        static void updateCategory()
        {
            using(var db = new NorthwindContext(optionBuilder.Options))
            {
                //search category by id
                var categoryExist = db.Categories.Find(9);
                if(categoryExist != null)
                {
                    categoryExist.CategoryName = "Chinnes Food";
                    categoryExist.Description = "Gurita";

                    db.Categories.Update(categoryExist);
                    db.SaveChanges();
                    Console.WriteLine("Successed is your data edit");
                }
                else
                {
                    Console.WriteLine("Sorry your data is not exist");
                }
            }
        }//endUpdateCategory

        static void DeleteCategory()
        {
            using(var db = new NorthwindContext(optionBuilder.Options))
            {
                // db transaction
                using(IDbContextTransaction t = db.Database.BeginTransaction())
                {
                    var categoryRemove = db.Categories.Find(9);
                    if(categoryRemove == null)
                    {
                        Console.WriteLine("Sorry Your data is not exist");
                    }
                    else
                    {
                        db.Categories.Remove(categoryRemove);
                        db.SaveChanges();
                        t.Commit();
                    }
                   
                }
            } 
        }//endDeleteCategory

        private static void FilteringAndSorting()
        {
            using(var db = new NorthwindContext(optionBuilder.Options))
            {
                IEnumerable<Product> filterProduct = db.Products.Where(x => x.UnitPrice < 10M);
                filterProduct = filterProduct.OrderByDescending(x => x.UnitPrice);

                WriteLine("Product price less than $10");
                foreach(Product product in filterProduct)
                {
                    WriteLine("{0} : {1} Cost {2:$##0.00}",
                        product.ProductId, product.ProductName, product.UnitPrice);
                }
                WriteLine();
            }
        }//endFilteringAndSorting

        protected static void SelectFilterSorting()
        {
            using(var db = new NorthwindContext(optionBuilder.Options))
            {
                //Interface IQuery filtering  dilakukan di clien side
                IOrderedQueryable<Product> sortFilteredProducts = db.Products.OrderByDescending(p => p.UnitPrice);

                var attProducs = sortFilteredProducts
                    .Select(product => new
                    {
                        prodId = product.ProductId,
                        product.ProductName,
                        product.UnitPrice,
                        total = product.UnitPrice * product.UnitsInStock
                    });

                WriteLine("Product price less than $10");
                foreach (var product in attProducs)
                {
                    WriteLine("{0} : {1} Cost {2:$##0.00} {3: $#.##0.00}",
                       product.prodId, product.ProductName, product.UnitPrice, product.total);
                }
                WriteLine();
            }
             
        }

        private static void AgregateProduct()
        {
            using(var db = new NorthwindContext(optionBuilder.Options))
            {
                Console.WriteLine($"Total Price  {db.Products.Sum(p => p.UnitPrice)}");
            }
        }

        private static void GetProductSupplierDto()
        {
            using (var db = new NorthwindContext(optionBuilder.Options))
            {
                var resultSet = db.ProductSupplierDTOs
                                .FromSqlRaw("SELECT [p].[ProductID], [p].[ProductName], [c].[CategoryName], " +
                                " [s].[CompanyName],  [s].[Address] FROM[Products] AS[p] " +
                                "LEFT JOIN[Categories] AS[c] ON[p].[CategoryId] = [c].[CategoryId] " +
                                "LEFT JOIN[Suppliers] AS[s] ON[p].[SupplierId] = [s].[SupplierId]").ToList();

                foreach (var item in resultSet)
                {
                    Console.WriteLine($"{item.productId} | {item.productName} | {item.categoryName} | {item.address}");
                }
            }
        }

    }
}
