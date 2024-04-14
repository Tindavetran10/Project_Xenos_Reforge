using InventorySystem_and_Items;
using InventorySystem_and_Items.Data;
using UnityEngine.EventSystems;

namespace UI
{
    public class UICraftSlot : UIItemSlot
    {
        private void OnEnable() => UpdateSlot(item);

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            ItemDataEquipment craftData = item.data as ItemDataEquipment;

            InventoryManager.instance.CanCraft(craftData, craftData.craftingMaterials);

        }
    }
}