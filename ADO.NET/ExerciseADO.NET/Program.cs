using Microsoft.Data.SqlClient;
using System;

namespace ExerciseADO.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            // using SqlConnection connection = new SqlConnection("Server=.;Integrated Security=true");
            //connection.Open();

            //SqlCommand createDataBase = new SqlCommand("CREATE DATABASE MinionsDB", connection);
            //createDataBase.ExecuteNonQuery();

            //using SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=true");
            //connection.Open();

            //SqlCommand tableCountries=new SqlCommand(@"CREATE TABLE Countries(
            //                                         Id INT PRIMARY KEY IDENTITY,
            //                                         Name NVARCHAR(50) NOT NULL)",connection);
            //tableCountries.ExecuteNonQuery();

            //SqlCommand tableTowns = new SqlCommand(@"CREATE TABLE Towns(
            //                                        Id INT PRIMARY KEY IDENTITY,
            //                                        Name NVARCHAR(50) NOT NULL,
            //                                        CountryCode INT REFERENCES Countries(Id))", connection);
            //tableTowns.ExecuteNonQuery();

            //SqlCommand tableMinions = new SqlCommand(@"CREATE TABLE Minions(
            //                                          Id INT PRIMARY KEY IDENTITY,
            //                                          Name NVARCHAR(50) NOT NULL,
            //                                          Age INT NOT NULL,
            //                                          TownId INT FOREIGN KEY REFERENCES Towns(Id))", connection);
            //tableMinions.ExecuteNonQuery();

            //SqlCommand tableEvilnessFactors = new SqlCommand(@"CREATE TABLE EvilnessFactors(
            //                                                Id INT PRIMARY KEY IDENTITY,
            //                                                Name NVARCHAR(50) NOT NULL,)", connection);
            //tableEvilnessFactors.ExecuteNonQuery();

            //SqlCommand tableVillains = new SqlCommand(@"CREATE TABLE Villains(
            //                                        Id INT PRIMARY KEY IDENTITY,
            //                                        Name NVARCHAR(50) NOT NULL,
            //                                        EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id)
            //                                        )", connection);
            //tableVillains.ExecuteNonQuery();

            //SqlCommand tableMinionsVillains = new SqlCommand(@"CREATE TABLE MinionsVillains(
            //                                                  MinionId INT FOREIGN KEY REFERENCES Minions(Id),
            //                                                  VillainId INT FOREIGN KEY REFERENCES Villains(Id),
            //                                                  CONSTRAINT PK_MinionIdVillainId PRIMARY KEY                                           (MinionId,VillainId)
            //                                                  )", connection);
            //tableMinionsVillains.ExecuteNonQuery();

            //SqlCommand insertInCountries = new SqlCommand(@"INSERT INTO Countries (Name) VALUES
            //                                                                         ('Bulgaria'),
            //                                                                         ('Turkey'),
            //                                                                         ('Germany'),
            //                                                                         ('Greece'),
            //                                                                         ('UK')", connection);
            //insertInCountries.ExecuteNonQuery();

            //SqlCommand insertInTowns = new SqlCommand(@"INSERT INTO Towns (Name,CountryCode) VALUES
            //                                                                         ('Varna',11),
            //                                                                         ('Istanbul',12),
            //                                                                         ('Dresden',13),
            //                                                                         ('Atina',14),
            //                                                                         ('London',15)", connection);
            //insertInTowns.ExecuteNonQuery();

            //SqlCommand insertInMinions = new SqlCommand(@"INSERT INTO Minions (Name,Age,TownId) VALUES
            //                                                                    ('Gosho',35,1),
            //                                                                       ('Vanq',26,2),
            //                                                                       ('Dancho',53,3),
            //                                                                       ('Ana',38,4),
            //                                                                       ('Ina',16,5)", connection);
            //insertInMinions.ExecuteNonQuery();

            //SqlCommand insertInEvilnessFactors = new SqlCommand(@"INSERT INTO EvilnessFactors (Name) VALUES
            //                                                                                 ('Super good'),
            //                                                                                 ('Good'),
            //                                                                                 ('Bad'),
            //                                                                                 ('Evil'),
            //                                                                                 ('Super evil')",                                           connection);
            //insertInEvilnessFactors.ExecuteNonQuery();

            //SqlCommand insertInVillains = new SqlCommand(@"INSERT INTO Villains (Name,EvilnessFactorId) VALUES
            //                                                                    ('Sasho',2),
            //                                                                    ('Pesho',4),
            //                                                                    ('Qnko',5),
            //                                                                    ('Grinch',1),
            //                                                                    ('Astor',3)", connection);
            //insertInVillains.ExecuteNonQuery();

            //SqlCommand insertInMinionsVillains = new SqlCommand(@"INSERT INTO MinionsVillains                                                                           (MinionId,VillainId) VALUES
            //                                                        (1,2),
            //                                                        (3,4),
            //                                                        (2,5),
            //                                                        (4,1),
            //                                                        (5,3)", connection);
            //insertInMinionsVillains.ExecuteNonQuery();

            using SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDBDemo;Integrated Security = true");
            connection.Open();

            //SqlCommand countMinForVillain = new SqlCommand(@"SELECT Name, COUNT(*) as CountMinion FROM MinionsVillains
            //                                                JOIN Villains ON Villains.Id = MinionsVillains.VillainId
            //                                                GROUP BY VillainId, Name
            //                                                HAVING COUNT(*) > 3
            //                                                ORDER BY COUNT(*) DESC", connection);
            //using SqlDataReader resultFromQuery = countMinForVillain.ExecuteReader();

            //while (resultFromQuery.Read())
            //{
            //    Console.WriteLine($"{resultFromQuery["Name"]} - {resultFromQuery["CountMinion"]}");
            //}

            //int idFromVillain = int.Parse(Console.ReadLine());
            //SqlCommand searchVillainName = new SqlCommand(@"SELECT Name From Villains
            //                                                  WHERE Id = @idFromVillain", connection);

            //searchVillainName.Parameters.AddWithValue("@idFromVillain", idFromVillain);

            //string foundVillainName = (string)searchVillainName.ExecuteScalar();

            //if (foundVillainName != null)
            //{
            //    Console.WriteLine($"Villain: {foundVillainName}");

            //    SqlCommand villainsMinions = new SqlCommand(@"SELECT m.Name,m.Age FROM MinionsVillains as mv
            //                                                JOIN Minions as m ON m.Id = mv.MinionId
            //                                                JOIN Villains as v ON v.Id = mv.VillainId
            //                                                WHERE mv.VillainId = @idFromVillain
            //                                                ORDER BY m.Name ASC", connection);

            //    villainsMinions.Parameters.AddWithValue("@idFromVillain", idFromVillain);

            //    string foundedMinionsName = (string)villainsMinions.ExecuteScalar();

            //    if (foundedMinionsName != null)
            //    {
            //        using var minions = villainsMinions.ExecuteReader();

            //        int count = 1;
            //        while (minions.Read())
            //        {
            //            Console.WriteLine($"{count++}. {minions["Name"]} {minions["Age"]}");
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("(no minions)");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine($"No villain with ID {idFromVillain} exists in the database.");
            //}

        }
    }
}
