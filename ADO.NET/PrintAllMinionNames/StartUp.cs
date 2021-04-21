using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace PrintAllMinionNames
{
    internal class StartUp
    {
        private static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDBDemo;Integrated Security=true");
            connection.Open();

            SqlCommand minionsName = new SqlCommand("SELECT Name FROM Minions", connection);
            var resultFromQuery = minionsName.ExecuteReader();
            List<string> minions = new List<string>();

            while (resultFromQuery.Read())
            {
                minions.Add(resultFromQuery["Name"].ToString());
            }

            for (int i = 0; i < minions.Count / 2; i++)
            {
                string current = minions[i];
                Console.WriteLine(current);

                string lastMinion = minions[minions.Count - 1 - i];
                Console.WriteLine(lastMinion);
            }

            if (minions.Count % 2 != 0)
            {
                int middle = (int)Math.Ceiling(minions.Count / 2.0);
                Console.WriteLine(minions[middle]);
            }
        }
    }
}