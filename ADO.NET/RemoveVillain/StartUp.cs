using Microsoft.Data.SqlClient;
using System;

namespace RemoveVillain
{
    class StartUp
    {
        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            using SqlConnection connection = new SqlConnection("Server=.;Database=MinionsDBDemo;Integrated Security=true");
            connection.Open();

            using SqlTransaction sqlTransaction = connection.BeginTransaction();

            SqlCommand searchedVillainName = new SqlCommand(@"SELECT Name FROM Villains
                                                         WHERE Id = @Id", connection);

            searchedVillainName.Parameters.AddWithValue("Id", villainId);
            searchedVillainName.Transaction = sqlTransaction;
            string resultFromQuery = (string)searchedVillainName.ExecuteScalar();

            if (resultFromQuery != null)
            {
                try
                {
                    SqlCommand removeFromMinionVillaint = new SqlCommand(@"DELETE FROM MinionsVillains 
                                                                        WHERE VillainId = @Id", connection);
                    removeFromMinionVillaint.Parameters.AddWithValue("@Id", villainId);
                    removeFromMinionVillaint.Transaction = sqlTransaction;
                    int removerRows = removeFromMinionVillaint.ExecuteNonQuery();

                    SqlCommand deleteVillain = new SqlCommand(@"DELETE FROM Villains WHERE Id = @Id", connection);
                    deleteVillain.Parameters.AddWithValue("@Id", villainId);
                    deleteVillain.Transaction = sqlTransaction;
                    deleteVillain.ExecuteNonQuery();

                    sqlTransaction.Commit();

                    Console.WriteLine($"{resultFromQuery} was deleted.");
                    Console.WriteLine($"{removerRows} minions were released.");
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    try
                    {
                        sqlTransaction.Rollback();
                    }
                    catch (Exception rollBackEx)
                    {
                        Console.WriteLine(rollBackEx.Message);

                    }
                }
            }
            else
            {
                Console.WriteLine("No such villain was found.");
            }
        }
    }
}
