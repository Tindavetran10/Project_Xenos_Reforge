using System;
using InventorySystem_and_Items;
using InventorySystem_and_Items.Data;
using UnityEngine;

namespace UI
{
    public class UIEquipmentSlot : UIItemSlot
    {
        public EquipmentType slotType;

        private void OnValidate()
        {
            gameObject.name = "Equipment Slot - " + slotType;
        }
        
        public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
        {
            InventoryManager.Instance.UnequipItem(item.data as ItemDataEquipment);
            InventoryManager.Instance.AddItem(item.data as ItemDataEquipment);
            CleanUpSlot();
        }
    }
}