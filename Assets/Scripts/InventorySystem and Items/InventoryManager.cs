using System.Collections.Generic;
using InventorySystem_and_Items.Data;
using UI;
using UnityEngine;

namespace InventorySystem_and_Items
{
    public class InventoryManager : MonoBehaviour
    {
        // Ensure only one instance of InventoryManager exists
        public static InventoryManager Instance { get; private set; }
        
        public List<InventoryItem> Inventory { get; private set; }
        private Dictionary<ItemData, InventoryItem> _inventoryDictionary;

        public List<InventoryItem> equipment;
        public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;
        
        public List<InventoryItem> Material{ get; private set; }
        private Dictionary<ItemData, InventoryItem> _materialDictionary;
        
        [Header("Inventory UI")] 
        [SerializeField] private Transform inventorySlotParent;
        [SerializeField] private Transform materialSlotParent;
        
        
        private UIItemSlot[] _inventoryItemSlots;
        private UIItemSlot[] _materialItemSlots;
        
        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else Instance = this;
        }

        private void Start()
        {
            Inventory = new List<InventoryItem>();
            _inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
            
            Material = new List<InventoryItem>();
            _materialDictionary = new Dictionary<ItemData, InventoryItem>();
            
            _inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
            _materialItemSlots = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
        }

        private void UpdateSlotUI()
        {
            for (int i = 0; i < Inventory.Count; i++) 
                _inventoryItemSlots[i].UpdateSlot(Inventory[i]);
            
            for (int i = 0; i < Material.Count; i++) 
                _materialItemSlots[i].UpdateSlot(Material[i]);
        }

        public void AddItem(ItemData newItemData)
        {
            if(newItemData.itemType == ItemType.Equipment) 
                AddToInventory(newItemData);
            else if (newItemData.itemType == ItemType.Material) 
                AddToMaterial(newItemData);
            
            UpdateSlotUI();
        }

        private void AddToMaterial(ItemData newItemData)
        {
            if(_materialDictionary.TryGetValue(newItemData, out InventoryItem existingItem))
                existingItem.AddStack();
            else
            {
                var newItem = new InventoryItem(newItemData);
                Material.Add(newItem);
                _materialDictionary.Add(newItemData, newItem);
            }
        }

        private void AddToInventory(ItemData newItemData)
        {
            // If item already exists in inventory, add stack
            if (_inventoryDictionary.TryGetValue(newItemData, out InventoryItem existingItem))
                existingItem.AddStack();
            // If item does not exist in inventory, create new item
            else
            {
                // Create new item and add to inventory
                var newItem = new InventoryItem(newItemData);
                Inventory.Add(newItem);
                _inventoryDictionary.Add(newItemData, newItem);
            }
        }

        public void RemoveItem(ItemData itemData)
        {
            // If item exists in inventory, remove stack
            if (_inventoryDictionary.TryGetValue(itemData, out InventoryItem existingItem))
            {
                // If stack size is 1, remove item from inventory
                if (existingItem.stackSize <= 1)
                {
                    Inventory.Remove(existingItem);
                    _inventoryDictionary.Remove(itemData);
                }
                // If stack size is greater than 1, remove stack
                else existingItem.RemoveStack();
            }
            
            if(_materialDictionary.TryGetValue(itemData, out InventoryItem existingMaterial))
            {
                if (existingMaterial.stackSize <= 1)
                {
                    Material.Remove(existingMaterial);
                    _materialDictionary.Remove(itemData);
                }
                else existingMaterial.RemoveStack();
            }
            
            UpdateSlotUI();
        }
    }
}
