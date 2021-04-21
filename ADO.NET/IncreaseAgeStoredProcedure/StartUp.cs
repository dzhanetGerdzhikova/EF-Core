using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Xml;

namespace IncreaseAgeStoredProcedure
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDBDemo;Integrated Security=true");
            connection.Open();

            int idMinion = int.Parse(Console.ReadLine());

            SqlCommand increaseAge = new SqlCommand("usp_GetOlder", connection);
            increaseAge.CommandType = CommandType.StoredProcedure;
            increaseAge.Parameters.AddWithValue("@MinionId", idMinion);
            increaseAge.ExecuteNonQuery();

            //SqlCommand setAgeMinion = new SqlCommand("UPDATE Minions SET Age +=1 WHERE Id=@id", connection);
            //setAgeMinion.Parameters.AddWithValue("@id", idMinion);
            //setAgeMinion.ExecuteNonQuery();

            SqlCommand minionInfo = new SqlCommand("SELECT Name,Age FROM Minions WHERE Id=@id", connection);
            minionInfo.Parameters.AddWithValue("@id", idMinion);
           using SqlDataReader result = minionInfo.ExecuteReader();

            while (result.Read())
            {
                Console.WriteLine($"{result["Name"]} - {result["Age"]} years old");
            }
        }
    }
}
