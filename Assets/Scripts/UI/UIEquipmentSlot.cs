using InventorySystem_and_Items;
using InventorySystem_and_Items.Data;

namespace UI
{
    public class UIEquipmentSlot : UIItemSlot
    {
        public EnumList.EquipmentType slotType;

        private void OnValidate() => gameObject.name = "Equipment Slot - " + slotType;

        #region OldCode for Removing Equipment
        /*public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
        {
            // Remove the item from the equipment slot when clicked on the slot Equipment UI
            InventoryManager.instance.UnequipItem(item.data as ItemDataEquipment);
            
            // Add the item back to the inventory 
            InventoryManager.instance.AddItem(item.data as ItemDataEquipment);
            CleanUpSlot();
        }*/
        #endregion

        protected override void ItemClicked()
        {
            base.ItemClicked();
            
            if(item == null || item.data == null) return;
            
            // Remove the item from the equipment slot when clicked on the slot Equipment UI
            InventoryManager.instance.UnequipItem(item.data as ItemDataEquipment);
            
            // Add the item back to the inventory 
            InventoryManager.instance.AddItem(item.data as ItemDataEquipment);
            CleanUpSlot();
        }
    }
}