using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace AddMinions
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDBDemo;Integrated Security = true");
            connection.Open();

            string[] info = Console.ReadLine().Split(':', StringSplitOptions.RemoveEmptyEntries);
            string[] minionInfo = info[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string nameMinion = minionInfo[0];
            int ageMinion = int.Parse(minionInfo[1]);
            string townNameOfMinion = minionInfo[2];

            string[] info2 = Console.ReadLine().Split(':', StringSplitOptions.RemoveEmptyEntries);
            string nameVillain = info2[1];

            int? townId = AddTown(connection, townNameOfMinion);

            AddMinion(connection, nameMinion, ageMinion, townId);

            AddVillain(connection, nameVillain);

            Console.WriteLine($"Successfully added {nameMinion} to be minion of {nameVillain}.");
        }

        private static void AddVillain(SqlConnection connection, string nameVillain)
        {
            SqlCommand villainInfo = new SqlCommand("SELECT * FROM Villains WHERE Name=@nameVillain", connection);
            villainInfo.Parameters.AddWithValue("@nameVillain", nameVillain);
            var result = villainInfo.ExecuteScalar();

            if (result == null)
            {
                SqlCommand addVillain = new SqlCommand("INSERT INTO Villains(Name,EvilnessFactorId) VALUES (@nameVillain,4)", connection);
                addVillain.Parameters.AddWithValue("@nameVillain", nameVillain);
                addVillain.ExecuteNonQuery();
                Console.WriteLine($"Villain {nameVillain} was added to the database.");
            }
        }

        private static int? AddTown(SqlConnection connection, string townNameOfMinion)
        {
            SqlCommand minionTowns = new SqlCommand("SELECT * FROM Towns WHERE Name=@townNameOfMinion", connection);
            minionTowns.Parameters.AddWithValue("@townNameOfMinion", townNameOfMinion);
            int? townId = (int?)minionTowns.ExecuteScalar();

            if (townId == null)
            {
                SqlCommand addTownInDb = new SqlCommand("INSERT INTO Towns (Name, CountryCode) VALUES (@townNameOfMinion,1)", connection);
                addTownInDb.Parameters.AddWithValue("@townNameOfMinion", townNameOfMinion);
                addTownInDb.ExecuteNonQuery();
                Console.WriteLine($"Town {townNameOfMinion} was added to the database.");

                SqlCommand minionTownId = new SqlCommand("SELECT Id FROM Towns WHERE Name=@townNameOfMinion", connection);
                minionTownId.Parameters.AddWithValue("@townNameOfMinion", townNameOfMinion);
                townId = (int?)minionTownId.ExecuteScalar();
            }

            return townId;
        }

        private static void AddMinion(SqlConnection connection, string nameMinion, int ageMinion, int? townId)
        {
            SqlCommand addMinion = new SqlCommand("INSERT INTO Minions (Name, Age, TownId) VALUES (@minionName, @minionAge, @townId)", connection);
            addMinion.Parameters.AddWithValue("@minionName", nameMinion);
            addMinion.Parameters.AddWithValue("@minionAge", ageMinion);
            addMinion.Parameters.AddWithValue("@townId", townId);
            addMinion.ExecuteNonQuery();
        }
    }
}
