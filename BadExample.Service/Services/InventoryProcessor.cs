using System;
using BadExample.Service.Interfaces;
using BadExample.Service.Models;

namespace BadExample.Service.Services
{
    public class InventoryProcessor : IInventoryProcessor
    {
        private readonly IDatabaseAccessor _databaseAccessor;
        public InventoryProcessor(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }
        public void ProcessLineItem(string line)
        {
            Console.WriteLine(line);
            InventoryItem item = MakeInventoryItem(line);
            if (item.IsValid())
            {
                _databaseAccessor.InsertInventory(item);
            }
        }

        private static InventoryItem MakeInventoryItem(string line)
        {
            var lineSplit = line.Split(',');
            if (lineSplit.Length != 5) return new InventoryItem();
            return new InventoryItem
            {
                Id = Convert.ToInt32(lineSplit[0]),
                Name = lineSplit[1],
                Type = lineSplit[2],
                Amount = Convert.ToInt32(lineSplit[3]),
                Cost = Convert.ToDecimal(lineSplit[4])
            };
        }
    }
}
