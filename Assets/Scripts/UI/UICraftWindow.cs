using InventorySystem_and_Items;
using InventorySystem_and_Items.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICraftWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Button craftButton;
        
        [SerializeField] private Image[] materialIcons;
        
        public void SetupCraftWindow(ItemDataEquipment itemData)
        {
            craftButton.onClick.RemoveAllListeners();
            
            foreach (var icon in materialIcons)
            {
                icon.color = Color.clear;
                icon.GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
            }

            for (var i = 0; i < itemData.craftingMaterials.Count; i++)
            {
                if(itemData.craftingMaterials.Count > materialIcons.Length)
                    Debug.LogWarning("You have more materials than the UI can display");
                
                materialIcons[i].sprite = itemData.craftingMaterials[i].data.icon;
                materialIcons[i].color = Color.white;
                
                var materialText = materialIcons[i].GetComponentInChildren<TextMeshProUGUI>();
                
                materialText.text = itemData.craftingMaterials[i].stackSize.ToString();
                materialText.color = Color.white;
            }
            
            itemIcon.sprite = itemData.icon;
            itemName.text = itemData.itemName;
            itemDescription.text = itemData.GetDescription();
            
            craftButton.onClick.AddListener(() => InventoryManager.instance.CanCraft(itemData, itemData.craftingMaterials));
        }
    }
}
