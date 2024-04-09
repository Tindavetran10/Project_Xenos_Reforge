using InventorySystem_and_Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIItemSlot : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemText;

        public InventoryItem item;
        
        public void UpdateSlot(InventoryItem newItem)
        {
            item = newItem;
            
            itemImage.color = Color.white;
            if(item != null)
            {
                itemImage.sprite = item.data.icon;
                itemText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
            }
        }
    }
}
