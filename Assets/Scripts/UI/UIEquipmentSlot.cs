using InventorySystem_and_Items;
using InventorySystem_and_Items.Data;

namespace UI
{
    public class UIEquipmentSlot : UIItemSlot
    {
        public EquipmentType slotType;

        private void OnValidate() => gameObject.name = "Equipment Slot - " + slotType;

        public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
        {
            // Remove the item from the equipment slot when clicked on the slot Equipment UI
            InventoryManager.Instance.UnequipItem(item.data as ItemDataEquipment);
            
            // Add the item back to the inventory 
            InventoryManager.Instance.AddItem(item.data as ItemDataEquipment);
            CleanUpSlot();
        }
    }
}