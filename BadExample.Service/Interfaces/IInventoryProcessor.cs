using BadExample.Service.Models;

namespace BadExample.Service.Interfaces
{
    public interface IInventoryProcessor
    {
        void ProcessLineItem(string line);
        InventoryItem MakeInventoryItem(string line);
    }
}