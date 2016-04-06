using System;
using System.Text.RegularExpressions;

namespace BadExample.Service.Models
{
    public class InventoryItem
    {
        public string Name { get; private set; }
        public int Id { get; private set; }
        public string Type { get; private set; }
        public int Amount { get; private set; }
        public decimal Cost { get; private set; }
        public InventoryItem()
        {
            Id = -1;
            Amount = 0;
            Cost = 0;
            Name = "";
            Type = "";
        }
        public InventoryItem(string id, string name, string type, string amount, string cost)
        {
            try
            {
                Id = Convert.ToInt32(id);
                Amount = Convert.ToInt32(amount);
                Cost = Convert.ToDecimal(cost);
                Name = name.Trim();
                Type = type.Trim();
            }
            catch
            {
                Id = -1;
                Amount = 0;
                Cost = 0;
                Name = "";
                Type = "";
            }
        }
    }
}