using System;
using InventorySystem_and_Items.Data;

namespace InventorySystem_and_Items
{
    [Serializable]
    public class InventoryItem
    {
        public ItemData data;
        public int stackSize;
        
        // Create a constructor to assign the item data and add a stack
        public InventoryItem(ItemData newItemData)
        {
            data = newItemData;
            AddStack();
        }

        public void AddStack() => stackSize ++;
        public void RemoveStack() => stackSize --;
    }
}