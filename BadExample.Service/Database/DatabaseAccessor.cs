using System;
using System.Configuration;
using System.Data.SqlClient;
using BadExample.Service.Interfaces;
using BadExample.Service.Models;

namespace BadExample.Service.Database
{
    public class DatabaseAccessor : IDatabaseAccessor
    {
        private readonly string _connectionString;
        public DatabaseAccessor()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }
        public void InsertInventory(InventoryItem item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command =
                    new SqlCommand("INSERT INTO Candy_Inventory (ID, Name, Type, Amount, Cost) values (@id, @name, @type, @amount, @cost)");
                command.Parameters.AddWithValue("id", item.Id);
                command.Parameters.AddWithValue("name", item.Name);
                command.Parameters.AddWithValue("type", item.Type);
                command.Parameters.AddWithValue("amount", item.Amount);
                command.Parameters.AddWithValue("cost", item.Cost);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int returnVal = Convert.ToInt32(reader[0]);
                    Console.WriteLine(returnVal);
                }
                reader.Close();
            }
        }
    }
}
