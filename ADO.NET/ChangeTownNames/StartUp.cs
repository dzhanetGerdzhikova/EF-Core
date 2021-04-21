using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace ChangeTownNames
{
    class StartUp
    {
        static void Main(string[] args)
        {
            string countryName = Console.ReadLine();

            using SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDBDemo;Integrated Security=true");
            connection.Open();

            SqlCommand searchCountry = new SqlCommand(@"SELECT t.Name FROM Towns AS t
                                                  JOIN Countries AS c ON c.Id = t.CountryCode
                                                  WHERE c.Name = @country", connection);
            searchCountry.Parameters.AddWithValue("@country", countryName);
           using var resulrFromQuery = searchCountry.ExecuteReader();
            List<string> towns = new List<string>();

            while (resulrFromQuery.Read())
            {
                towns.Add(resulrFromQuery["Name"].ToString());
            }

            if(towns.Count!=0)
            {
                Console.WriteLine($"{towns.Count} town names were affected. ");

                foreach (var currentTown in towns)
                {
                    Console.WriteLine($"[{currentTown.ToUpper()}]");
                }
            }
            else
            {
                Console.WriteLine("No town names were affected.");
            }
        }
    }
}
