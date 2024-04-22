using InventorySystem_and_Items.Data;

namespace UI
{
    public class UICraftSlot : UIItemSlot
    {
        private void OnEnable() => UpdateSlot(item);

        public void SetupCraftSlot(ItemDataEquipment itemData)
        {
            if(itemData== null) return;
            
            item.data = itemData;
            itemImage.sprite = itemData.icon;
            itemText.text = itemData.itemName;
            
            if(itemText.text.Length > 12)
                itemText.fontSize *= .7f;
            else itemText.fontSize = 20;
        }
        
        /*public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            var craftData = item.data as ItemDataEquipment;

            if (craftData != null) 
                InventoryManager.instance.CanCraft(craftData, craftData.craftingMaterials);
        }*/

        protected override void ItemClicked()
        {
            /*base.ItemClicked();
            if(item == null || item.data == null) return;
            
            var craftData = item.data as ItemDataEquipment;

            if (craftData == null) return;

            if (craftData != null) 
                InventoryManager.instance.CanCraft(craftData, craftData.craftingMaterials);*/
            UIManager.craftWindow.SetupCraftWindow(item.data as ItemDataEquipment);
        }
    }
}