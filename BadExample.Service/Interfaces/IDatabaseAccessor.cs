using BadExample.Service.Models;

namespace BadExample.Service.Interfaces
{
    public interface IDatabaseAccessor
    {
        void InsertInventory(InventoryItem item);
    }
}
