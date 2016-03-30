using System;
using System.Data.SqlClient;
using BadExample.Service.Interfaces;

namespace BadExample.Service.Services
{
    public class InventoryProcessor : IInventoryProcessor
    {
        private readonly string _connectionString;
        public InventoryProcessor(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void ProcessLineItem(string line)
        {
            Console.WriteLine(line);
            var lineSplit = line.Split(',');
            if (lineSplit.Length == 5)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command =
                        new SqlCommand("INSERT INTO Candy_Inventory (ID, Name, Type, Amount, Cost) values (@id, @name, @type, @amount, @cost)");
                    command.Parameters.AddWithValue("id", lineSplit[0]);
                    command.Parameters.AddWithValue("name", lineSplit[1]);
                    command.Parameters.AddWithValue("type", lineSplit[2]);
                    command.Parameters.AddWithValue("amount", lineSplit[3]);
                    command.Parameters.AddWithValue("cost", Convert.ToDouble(lineSplit[4]));

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
}
