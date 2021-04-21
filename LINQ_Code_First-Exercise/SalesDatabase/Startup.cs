using P03_SalesDatabase.Data;
using SalesDatabase.Data.Seeding;
using SalesDatabase.Data.Seeding.Contracts;
using SalesDatabase.IOManagement;
using SalesDatabase.IOManagement.Contracts;
using System;
using System.Collections.Generic;

namespace P03_SalesDatabase
{
  public  class Startup
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            var dataBase = new SalesContext();
            IWriter writer = new ConsoleWriter();

            dataBase.Database.EnsureDeleted();
            dataBase.Database.EnsureCreated();

            ICollection<ISeeder> seeders = new List<ISeeder>();
            seeders.Add(new ProductSeeder(dataBase, random, writer));
            seeders.Add(new StoreSeeder(dataBase, writer));

            foreach (var seeder in seeders)
            {
                seeder.Seed();
            }




        }
    }
}
