using Microsoft.Data.SqlClient;
using System;

namespace ADO.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            int userId = int.Parse(Console.ReadLine());
            string firstName = Console.ReadLine();

            using SqlConnection connection = new SqlConnection("Server=.;Database=Bank;Integrated Security = true");

            connection.Open();

            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM AccountHolders WHERE Id = @userId AND FirstName = @firstName", connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@firstName", firstName);

            int resultFromQuery =(int) command.ExecuteScalar();

            if (resultFromQuery > 0)
            {
                Console.WriteLine("This account exist.");
            }
            else 
            {
                Console.WriteLine( "Account doesn't exist.");
            }

        }
    }
}
