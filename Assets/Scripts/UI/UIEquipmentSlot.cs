using System;
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
    }
}