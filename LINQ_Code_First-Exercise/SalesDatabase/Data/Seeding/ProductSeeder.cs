using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;
using SalesDatabase.Data.Seeding.Contracts;
using SalesDatabase.IOManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesDatabase.Data.Seeding
{
  public  class ProductSeeder : ISeeder
    {
        private readonly Random random;
        private readonly SalesContext dbContext;
        private readonly IWriter writer;
        public ProductSeeder(SalesContext context,Random random,IWriter writer)
        {
            this.dbContext = context;
            this.random = random;
            this.writer = writer;
        }
        public void Seed()
        {
            ICollection<Product> products = new List<Product>();

            string[] names = new string[]
            { 
            "Banana",
            "Blueberriy",
            "Watermelon",
            "Kiwi",
            "Apple",
            "Orange",
            "Pomelo",
            "Grape",
            "Pear",
            "Peach",
            "Cherry",
            "Mango",
            };

            for (int i = 0; i < 50; i++)
            {
                int nameIndex = this.random.Next(0, names.Length);
                string currentProductName = names[nameIndex];
                double quantity = this.random.Next(20);
                decimal price = this.random.Next(200)*1.133m;

                Product product = new Product
                {
                    Name = currentProductName,
                    Quantity = quantity,
                    Price = price
                };

                products.Add(product);

                this.writer.WriteLine($"Product {product.Name} {product.Quantity} {product.Price}$ was added to the database!");
            }
            this.dbContext.Products.AddRange(products);

            this.dbContext.SaveChanges();
        }
    }
}
