using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;
using SalesDatabase.Data.Seeding.Contracts;
using SalesDatabase.IOManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesDatabase.Data.Seeding
{
  public  class StoreSeeder : ISeeder
    {
        private readonly SalesContext dbContext;
        private readonly IWriter writer;
        public StoreSeeder(SalesContext context, IWriter writer)
        {
            this.dbContext = context;
            this.writer = writer;
        }
        public void Seed()
        {
            Store[] stores = new Store[]
            {
               new Store{Name="Billa"},
               new Store{Name="Kaufland"},
               new Store{Name="Lidl"},
               new Store{Name="TMarket"},
               new Store{Name="365"},
               new Store{Name="KAM Market"},
               new Store{Name="Fantastiko"},
               new Store{Name="Hit"}
            };

           this.dbContext.Stores.AddRange(stores);

            this.dbContext.SaveChanges();

            this.writer.WriteLine($"{stores.Length} was added to the database.");
        }
    }
}
