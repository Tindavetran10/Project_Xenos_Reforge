using System;
using InventorySystem_and_Items;
using InventorySystem_and_Items.Data;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIItemSlot : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] protected Image itemImage;
        [SerializeField] protected TextMeshProUGUI itemText;
        
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

        public void CleanUpSlot()
        {
            item = null;
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            
            itemText.text = "";
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (item.data.itemType == ItemType.Equipment)
                InventoryManager.instance.EquipItem(item.data);
        }
    }
}
