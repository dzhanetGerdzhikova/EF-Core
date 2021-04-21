using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XmlFacade;
namespace ProductShop
{
    public class StartUp
    {
        public static object XMLHelper { get; private set; }

        public static void Main(string[] args)
        {
            using ProductShopContext dataBase = new ProductShopContext();

            //CreateDataBase();

            //Problem 1
            //string xmlUsers = File.ReadAllText("../../../Datasets/users.xml");
            //string result = ImportUsers(dataBase, xmlUsers);
            //Console.WriteLine(result);

            //Problem2
            //string xmlProducts = File.ReadAllText("../../../Datasets/products.xml");
            //string result = ImportProducts(dataBase, xmlProducts);
            //Console.WriteLine(result);

            //Problem 3
            //string xmlCategories = File.ReadAllText("../../../Datasets/categories.xml");
            //string result = ImportCategories(dataBase, xmlCategories);
            //Console.WriteLine(result);

            //Problem 4
            //string xmlProductsCategories = File.ReadAllText("../../../Datasets/categories-products.xml");
            //string result = ImportCategoryProducts(dataBase, xmlProductsCategories);
            //Console.WriteLine(result);

            //Problem 5
            //string products = GetProductsInRange(dataBase);
            //File.WriteAllText("../../../ResultsInXML/ProductShopResult/products-in-range.xml", products);

            //Problem 6
            //string soldProducts = GetSoldProducts(dataBase);
            //File.WriteAllText("../../../ResultsInXML/ProductShopResult/users-sold-products.xml", soldProducts);

            ////Problem 7
            //string categoryByProductsCount = GetCategoriesByProductsCount(dataBase);
            //File.WriteAllText("../../../ResultsInXML/ProductShopResult/categories-by-products.xml", categoryByProductsCount);

            //Problem 8
            string usersWithProducts = GetUsersWithProducts(dataBase);
            File.WriteAllText("../../../ResultsInXML/ProductShopResult/users-and-products.xml", usersWithProducts);
        }
        public static void CreateDataBase()
        {
            ProductShopContext dataBase = new ProductShopContext();
            dataBase.Database.EnsureDeleted();
            dataBase.Database.EnsureCreated();

            Console.WriteLine("Create DB!");
        }
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var usersDto = XMLConverter.Deserializer<ImportUserDto>(inputXml, "Users");

            var users = usersDto.Select(u => new User
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age
            }).ToArray();


            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Length}"; ;
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            ImportProductDto[] productsXml = XMLConverter.Deserializer<ImportProductDto>(inputXml, "Products");

            List<Product> products = productsXml.Select(p => new Product
            {
                Name = p.Name,
                Price = p.Price,
                SellerId = p.SellerId,
                BuyerId = p.BuyerId
            }).ToList();

            foreach (var product in products)
            {
                context.Products.Add(product);
                context.SaveChanges();
            }

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            ImportCategoriesDto[] categoriesXml = XMLConverter.Deserializer<ImportCategoriesDto>(inputXml, "Categories");

            List<Category> categories = categoriesXml
                .Where(c => c.Name != null)
                .Select(c => new Category
                {
                    Name = c.Name
                }).ToList();


            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {context.Categories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            ImportCategoriesProductsDto[] categoryProductXml = XMLConverter.Deserializer<ImportCategoriesProductsDto>(inputXml, "CategoryProducts");

            List<CategoryProduct> categoryProduct = categoryProductXml
                .Where(cp =>
                    context.Categories.Any(c => c.Id == cp.CategoryId)
             && context.Products.Any(p => p.Id == cp.ProductId))
                 .Select(cp => new CategoryProduct
                 {
                     CategoryId = cp.CategoryId,
                     ProductId = cp.ProductId
                 }).ToList();


            context.CategoryProducts.AddRange(categoryProduct);
            context.SaveChanges();

            return $"Successfully imported {categoryProduct.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            ExportProductInfoDto[] products = context.Products
                     .Where(p => p.Price >= 500 && p.Price <= 1000)
                     .Select(p => new ExportProductInfoDto
                     {
                         Name = p.Name,
                         Price = p.Price,
                         BuyerName = p.Buyer.FirstName + " " + p.Buyer.LastName
                     })
                     .OrderBy(p => p.Price)
                     .Take(10)
                     .ToArray();

            string xmlString = XMLConverter.Serialize(products, "Products");

            return xmlString;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            List<ExportUsersCountSoldProductsDto> users = context.Users
                .Where(u => u.ProductsSold.Any())

                .Select(u => new ExportUsersCountSoldProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserProducts = u.ProductsSold.Select(p => new UserProductDto
                    {
                        Name = p.Name,
                        Price = p.Price
                    }).ToArray()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ToList();

            string usersXml = XMLConverter.Serialize(users, "Users");

            return usersXml;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            List<ExportCategoryByProductDto> categories = context.Categories
                .Select(c => new ExportCategoryByProductDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(p => p.Product.Price),
                    TotalSum = c.CategoryProducts.Select(cp => cp.Product.Price).Sum()
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalSum)
                .ToList();

            string categoriXml = XMLConverter.Serialize(categories, "Categories");

            return categoriXml;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users.Where(u => u.ProductsSold.Any())
                  .Select(u => new ExportUsersWithSoldProductsDto
                  {
                      FirstName = u.FirstName,
                      LastName = u.LastName,
                      Age = u.Age,
                      SoldProduct = new ExportSoldProduct
                      {
                          Count = u.ProductsSold.Count(),
                          SoldProducts = u.ProductsSold.Select(p => new ExportProducts
                          {
                              Name = p.Name,
                              Price = p.Price
                          })
                          .OrderByDescending(p=>p.Price)
                          .ToArray()
                      }
                  }).OrderByDescending(u => u.SoldProduct.Count)
                  .Take(10)
                  .ToList();

            var result = new ExportUsersDto
            {
                Count = context.Users.Count(u=>u.ProductsSold.Any()),
                Users = users.ToArray()
            };

           string usersResult = XMLConverter.Serialize(result, "Users");

            return usersResult;
        }
    }
}