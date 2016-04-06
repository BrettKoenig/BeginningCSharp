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
            InventoryItem item = MakeInventoryItem(line);
            if (item.Id > 0)
            {
                _databaseAccessor.InsertInventory(item);
            }
        }
        public InventoryItem MakeInventoryItem(string line)
        {
            var lineSplit = line.Split(',');
            return lineSplit.Length != 5 ? new InventoryItem() : new InventoryItem(lineSplit[0], lineSplit[1], lineSplit[2], lineSplit[3], lineSplit[4]);
        }
    }
}
