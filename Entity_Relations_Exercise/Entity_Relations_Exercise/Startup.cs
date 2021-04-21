using P01_StudentSystem.Data;
using P03_FootballBetting.Data;
using System;

namespace 	P03_FootballBetting 
{
   public class Startup
    {
      public  static void Main(string[] args)
        {
            //var dataBase = new FootballBettingContext();
            //dataBase.Database.EnsureDeleted();
            //dataBase.Database.EnsureCreated();

            var dataBase = new StudentDbContext();
            dataBase.Database.EnsureDeleted();
            dataBase.Database.EnsureCreated();
        }
    }
}
