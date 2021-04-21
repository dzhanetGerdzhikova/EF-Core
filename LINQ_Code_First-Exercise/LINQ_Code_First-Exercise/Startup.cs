using P01_HospitalDatabase.Data;
using System;

namespace P01_HospitalDatabase 
{
   public class Startup
    {
        static void Main(string[] args)
        {
            var dataBade = new HospitalContext();
            dataBade.Database.EnsureDeleted();
            dataBade.Database.EnsureCreated();

        }
    }
}
