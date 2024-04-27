using System.Collections.Generic;
using _Scripts.Player.Input;
using InventorySystem_and_Items;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class UIItemSlot : MonoBehaviour
    {
        [SerializeField] protected Image itemImage;
        [SerializeField] protected TextMeshProUGUI itemText;
        [SerializeField] private InputManager inputManager;

        protected UIManager UIManager;
        public InventoryItem item;
        
        private UnityAction _itemSlotClickAction;
        
        private void Start()
        {
            UIManager = GetComponentInParent<UIManager>();
            
            inputManager.ItemSlotClickEvent += () =>
            {
                if (IsPointerOverUIObject())
                    ItemClicked();
            };
            
            inputManager.ItemSlotClickEvent += _itemSlotClickAction;
        }
        
        protected virtual void OnDestroy() => inputManager.ItemSlotClickEvent -= _itemSlotClickAction;

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

        #region OldCode for Selecting Equipment
        // Inherited from IPointerDownHandler
        /*public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (item.data.itemType == ItemType.Equipment)
                InventoryManager.instance.EquipItem(item.data);
        }*/
        #endregion
        
        private bool IsPointerOverUIObject()
        {
            // Check if the UIItemSlot object has been destroyed
            if (this == null)
                return false;
            
            //Get the mouse position
            var mousePosition = Mouse.current.position.ReadValue();
            
            // Create a raycast results list
            var results = new List<RaycastResult>();

            // Create a pointer event data object
            var eventData = new PointerEventData(EventSystem.current)
            {position = mousePosition};

            // Raycast using the graphics ray-caster and mouse position
            EventSystem.current.RaycastAll(eventData, results);

            // Return true if the results contain the UIItemSlot's UI element
            return results.Exists(result => result.gameObject == gameObject);
        }

        protected virtual void ItemClicked()
        {
            if(item == null || item.data == null) return;
            
            if (item.data.itemType == EnumList.ItemType.Equipment)
                InventoryManager.instance.EquipItem(item.data);
        }
    }
}
