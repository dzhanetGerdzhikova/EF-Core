using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IncreaseMinionAge
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDBDemo;Integrated Security=true");
            connection.Open();

            int[] input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            for (int i = 0; i < input.Length; i++)
            {
                int currentIdMinion = input[i];

                SqlCommand setAgeOfMinion = new SqlCommand(@"UPDATE Minions SET Age += 1
                                                            WHERE Id = @minionId", connection);
                setAgeOfMinion.Parameters.AddWithValue("@minionId", currentIdMinion);
                setAgeOfMinion.ExecuteNonQuery();

                SqlCommand getMinionName = new SqlCommand(@"SELECT Name FROM Minions WHERE Id = @minionId", connection);
                getMinionName.Parameters.AddWithValue("@minionId", currentIdMinion);

                string[] name = getMinionName.ExecuteScalar().ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries);
              
                for (int x = 0; x < name.Length; x++)
                {
                    name[x] = name[x][0].ToString().ToUpper() + name[x].Substring(1);
                }

               string newName = string.Join(' ', name);

                SqlCommand setNameMinion = new SqlCommand("UPDATE Minions SET Name = @NewName WHERE Id =                                                   @Id", connection);
                setNameMinion.Parameters.AddWithValue("@NewName", newName);
                setNameMinion.Parameters.AddWithValue("@Id", currentIdMinion);
                setNameMinion.ExecuteNonQuery();
            }
            SqlCommand updateInfo = new SqlCommand("SELECT Name, Age From Minions", connection);
            using var result = updateInfo.ExecuteReader();

            while (result.Read())
            {
                Console.WriteLine($"{result["Name"]} {result["Age"]}");
            }
        }
    }
}