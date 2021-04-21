using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTO;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static object stringBuilder;

        public static void Main(string[] args)
        {
            var db = new ProductShopContext();
            InitializationMapper();

            //Problem1
            //CreateDataBase(db);

            //Problem2
            //var jsonString = File.ReadAllText("../../../Datasets/users.json");
            //var result = ImportUsers(db, jsonString);
            //Console.WriteLine(result);

            //Problem3
            //string jasonInput = File.ReadAllText("../../../Datasets/products.json");
            //var result = ImportProducts(db, jasonInput);
            //Console.WriteLine(result);

            //Problem 4
            //string inputJson = File.ReadAllText("../../../Datasets/categories.json");
            //var result4 = ImportCategories(db, inputJson);
            //Console.WriteLine(result);

            //Problem 5
            //string jsonInput = File.ReadAllText("../../../Datasets/categories-products.json");
            //var result = ImportCategoryProducts(db, jsonInput);
            //Console.WriteLine(result);

            //string json = GetProductsInRange(db);
            //Anonimous
            //File.WriteAllText("../../../Datasets/ResultsInJSON/GetProductsInRange", json);
            //DTO object
            //File.WriteAllText("../../../Datasets/ResultsInJSON/GetProductsInRangeDTO", json);
            //AutoMapper
            //File.WriteAllText("../../../Datasets/ResultsInJSON/GetProductsInRangeWithAutoMapper", json);

            //Problem 6
            //string json = GetSoldProducts(db);
            //Anonimous
            //File.WriteAllText("../../../Datasets/ResultsInJSON/GetSoldProducts", json);
            //DTO
            //File.WriteAllText("../../../Datasets/ResultsInJSON/GetSoldProductsDto", json);
            //AutoMapper
            //File.WriteAllText("../../../Datasets/ResultsInJSON/GetSoldProductsAutoMapper", json);

            //Problem 7
            //string json = GetCategoriesByProductsCount(db);
            //Anonimous
            //File.WriteAllText("../../../Datasets/ResultsInJSON/GetCategoriesByProductsCount", json);
            //DTO object 
            //File.WriteAllText("../../../Datasets/ResultsInJSON/GetCategoriesByProductsCountDto", json);
            //AutoMapper
            //File.WriteAllText("../../../Datasets/ResultsInJSON/GetCategoriesByProductsCountAutoMapper", json);

            //Problem 8
            //string json = GetUsersWithProducts(db);
            //Anonimous object
            //File.WriteAllText("../../../Datasets/ResultsInJSON/GetUsersWithProducts", json);
            //DTO object
            //File.WriteAllText("../../../Datasets/ResultsInJSON/GetUsersWithProdutsDto", json);
            //AutoMapper
        }
        public static void InitializationMapper()
        {
            Mapper.Initialize(config => { config.AddProfile<ProductShopProfile>(); });
        }

        public static void CreateDataBase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            Console.WriteLine("DataBase was created!");
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {

            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(inputJson)
                  .Where(x => x.Name != null).ToList();

            context.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            List<CategoryProduct> categoriesProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            //Anonimous object
            //var products = context.Products
            //      .Where(p => p.Price >= 500 && p.Price <= 1000)
            //      .Select(p => new { name = p.Name, price = p.Price.ToString("f2"), seller = p.Seller.FirstName + " " + p.Seller.LastName })
            //      .OrderBy(p => p.price)
            //      .ToList();

            //string result = JsonConvert.SerializeObject(products,Formatting.Indented);

            //return result;

            //DTO object
            //var products1 = context.Products
            //      .Where(p => p.Price >= 500 && p.Price <= 1000)
            //      .Select(p => new ProductsInRangeDto{ 
            //          Name=p.Name,
            //          Price=p.Price,
            //          SellerName=p.Seller.FirstName+" "+p.Seller.LastName
            //      })
            //      .OrderBy(p => p.Price)
            //      .ToList();

            //string result1 = JsonConvert.SerializeObject(products1, Formatting.Indented);

            //return result1;

            //AutoMapper
            var products2 = context.Products
                 .Where(p => p.Price >= 500 && p.Price <= 1000)
                 .OrderBy(p => p.Price)
                 .ProjectTo<ProductsInRangeDto>()
                 .ToList();

            string result2 = JsonConvert.SerializeObject(products2, Formatting.Indented);

            return result2;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            //Anonimous object
            //var users = context.Users
            //    .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            //     .Select(u => new
            //     {
            //         firstName = u.FirstName,
            //         lastName = u.LastName,
            //         soldProducts = u.ProductsSold
            //         .Where(p=>p.Buyer !=null)
            //         .Select(p => new
            //         {
            //             name = p.Name,
            //             price = p.Price,
            //             buyerFirstName = p.Buyer.FirstName,
            //             buyerLastName = p.Buyer.LastName
            //         })
            //         .ToList()
            //     })
            //     .OrderBy(u => u.lastName)
            //     .ThenBy(u => u.firstName)
            //     .ToList();

            //string result = JsonConvert.SerializeObject(users, Formatting.Indented);

            //return result;

            //DTO object

            //var users = context.Users
            //    .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            //    .OrderBy(u => u.LastName)
            //     .ThenBy(u => u.FirstName)
            //     .Select(u => new SoldProductsDto
            //     {
            //         FirstName = u.FirstName,
            //         LastName = u.LastName,
            //         Products = u.ProductsSold
            //         .Where(p => p.Buyer != null)
            //         .Select(p => new ProductDto
            //         {
            //             Name = p.Name,
            //             Price = p.Price,
            //             BuyerFirstName = p.Buyer.FirstName,
            //             BuyerLastName = p.Buyer.LastName
            //         })
            //         .ToArray()
            //     })
            //     .ToList();

            //string result1 = JsonConvert.SerializeObject(users, Formatting.Indented);

            //return result1;

            //AutoMapper
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                 .ThenBy(u => u.FirstName)
                 .ProjectTo<SoldProductsDto>()
                 .ToList();

            string result = JsonConvert.SerializeObject(users, Formatting.Indented);

            return result;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            //Anonimous object
            //var categories = context.Categories
            //    .Select(c => new
            //    {
            //        category = c.Name,
            //        productsCount = c.CategoryProducts.Count(),
            //        averagePrice = c.CategoryProducts.Average(cp => cp.Product.Price).ToString("f2"),
            //        totalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price).ToString("f2")
            //    })
            //    .OrderByDescending(c => c.productsCount)
            //    .ToList();

            //string result = JsonConvert.SerializeObject(categories, Formatting.Indented);

            //return result;

            //DTO object
            //var categories = context.Categories
            //   .Select(c => new CategoryDto
            //   {
            //       Name = c.Name,
            //       CountProduct = c.CategoryProducts.Count(),
            //       AvrPrice = c.CategoryProducts.Average(cp => cp.Product.Price).ToString("f2"),
            //       TotalSum = c.CategoryProducts.Sum(cp => cp.Product.Price).ToString("f2")
            //   })
            //   .OrderByDescending(c => c.CountProduct)
            //   .ToList();

            //string result1 = JsonConvert.SerializeObject(categories, Formatting.Indented);

            //return result1;

            //AutoMapper
            var categories = context.Categories
              .ProjectTo<CategoryDto>()
              .OrderByDescending(c => c.CountProduct)
              .ToList();

            string result2 = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return result2;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            //Anonimous

            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Where(p => p.Buyer != null).Count(),
                        products = u.ProductsSold.Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            })
                    }
                })
                .OrderByDescending(u => u.soldProducts.count)
                .ToList();

            var resultObj = new
            {
                usersCount = users.Count(),
                users = users
            };

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            string result = JsonConvert.SerializeObject(resultObj, settings);

            return result;

            // DTO object

            //var users = context.Users.Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            //    .Select(u => new SellerDto
            //    {
            //        LastName = u.LastName,
            //        Age = u.Age,
            //        SoldProducts = new SoldProductOfSellerDto
            //        {
            //            CountSoldProducts = u.ProductsSold.Where(p => p.Buyer != null).Count(),
            //            Products =( ProductOfSellerDto[]) u.ProductsSold.Where(p => p.Buyer != null)
            //            .Select(p => new ProductOfSellerDto
            //            {
            //                Name = p.Name,
            //                Price = p.Price
            //            }).ToArray()
            //        }
            //    })
            //    .OrderByDescending(u => u.SoldProducts)
            //    .ToArray();


            //var resultObj = new UsersDto
            //{
            //    CountUsers = users.Count(),
            //    Sellers =(SellerDto[]) users
            //};

            //JsonSerializerSettings settings = new JsonSerializerSettings()
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    Formatting = Formatting.Indented
            //};

            //string result = JsonConvert.SerializeObject(resultObj, settings);

            //return result;
            //AutoMapper
        }
    }
}