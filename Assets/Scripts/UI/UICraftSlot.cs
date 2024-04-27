using InventorySystem_and_Items.Data;

namespace UI
{
    public class UICraftSlot : UIItemSlot
    {
        //private void OnEnable() => UpdateSlot(item);

        public void SetupCraftSlot(ItemDataEquipment itemData)
        {
            if(itemData== null) return;
            
            item.data = itemData;
            itemImage.sprite = itemData.icon;
            itemText.text = itemData.itemName;
            
            if(itemText.text.Length > 10)
                itemText.fontSize *= .7f;
            else itemText.fontSize = 20;
        }
        
        protected override void ItemClicked() => 
            UIManager.craftWindow.SetupCraftWindow(item.data as ItemDataEquipment);
    }
}