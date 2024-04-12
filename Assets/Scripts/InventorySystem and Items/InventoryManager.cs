using System.Collections.Generic;
using System.Linq;
using InventorySystem_and_Items.Data;
using UI;
using UnityEngine;

namespace InventorySystem_and_Items
{
    public class InventoryManager : MonoBehaviour
    {
        // Ensure only one instance of InventoryManager exists
        public static InventoryManager Instance { get; private set; }

        public List<InventoryItem> inventory;
        private Dictionary<ItemData, InventoryItem> _inventoryDictionary;

        public List<InventoryItem> material;
        private Dictionary<ItemData, InventoryItem> _materialDictionary;

        public List<InventoryItem> equipment;
        private Dictionary<ItemDataEquipment, InventoryItem> _equipmentDictionary;
        
        [Header("Inventory UI")] 
        [SerializeField] private Transform inventorySlotParent;
        [SerializeField] private Transform materialSlotParent;
        [SerializeField] private Transform equipmentSlotParent;
        
        private UIItemSlot[] _inventoryItemSlots;
        private UIItemSlot[] _materialItemSlots;
        private UIEquipmentSlot[] _equipmentItemSlots;
        
        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else Instance = this;
        }

        private void Start()
        {
            inventory = new List<InventoryItem>();
            _inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
            
            material = new List<InventoryItem>();
            _materialDictionary = new Dictionary<ItemData, InventoryItem>();

            equipment = new List<InventoryItem>();
            _equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();
            
            _inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
            _materialItemSlots = materialSlotParent.GetComponentsInChildren<UIItemSlot>();
            _equipmentItemSlots = equipmentSlotParent.GetComponentsInChildren<UIEquipmentSlot>();
        }

        public void EquipItem(ItemData itemData)
        {
            var newEquipment = itemData as ItemDataEquipment;
            var newItem = new InventoryItem(newEquipment);
            
            PreventPlayerEquipWeaponAlreadyEquipped(newEquipment);

            // Add new equipment (the same one) to equipment list
            equipment.Add(newItem);
            
            // Add new equipment to equipment dictionary
            if (newEquipment != null)
            {
                _equipmentDictionary.Add(newEquipment, newItem);
                
                // Modify player stats when player equips an equipment
                newEquipment.AddModifiers();
            }

            // Remove item from inventory when the player equips it
            RemoveItem(itemData);
            // Update the equipment slot UI
            UpdateSlotUI();
        }

        private void PreventPlayerEquipWeaponAlreadyEquipped(ItemDataEquipment newEquipment)
        {
            ItemDataEquipment oldEquipment = null;

            // Check if equipment of same type is already equipped
            foreach (var item 
                     in _equipmentDictionary.Where(item 
                         => newEquipment != null && item.Key.equipmentType == newEquipment.equipmentType)) 
                oldEquipment = item.Key;

            #region OldCodeForCheckingOldEquipment
            /*foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in _equipmentDictionary)
            {
                if(newEquipment != null && item.Key.equipmentType == newEquipment.equipmentType)
                    itemToRemove = item.Key;
            }*/
            #endregion

            // If equipment of same type is already equipped, remove the current equipment and add it to inventory
            // Equip the new equipment from the inventory
            if(oldEquipment != null)
            {
                // Remove the current equipment from equipment list
                UnequipItem(oldEquipment);
                
                // Add the equipment to inventory
                AddItem(oldEquipment);
            }
        }
        
        public void UnequipItem(ItemDataEquipment itemToRemove)
        {
            // If equipment exists in equipment list, remove it
            if(_equipmentDictionary.TryGetValue(itemToRemove, out var existingItem))
            {
                // Remove equipment from equipment list
                equipment.Remove(existingItem);
                // Remove equipment from equipment dictionary
                _equipmentDictionary.Remove(itemToRemove);
                // Remove modifiers from player stats when player unequipped an equipment
                itemToRemove.RemoveModifiers();
            }
        }

        private void UpdateSlotUI()
        {
            #region OldCodeUpdateSlot
            /*for (var i = 0; i < inventory.Count; i++)
                _inventoryItemSlots[i].UpdateSlot(inventory[i]);

            for (var i = 0; i < material.Count; i++)
                _materialItemSlots[i].UpdateSlot(material[i]);*/
            #endregion

            #region OldCodeForCleanUpSlots
            /*for(int i = 0; i < _inventoryItemSlots.Length; i++)
                _inventoryItemSlots[i].CleanUpSlot();

            for(int i = 0; i < _materialItemSlots.Length; i++)
                _materialItemSlots[i].CleanUpSlot();*/
            #endregion
            
            ChangeEquipmentItem();
            
            CleanUpSlots(_inventoryItemSlots);
            CleanUpSlots(_materialItemSlots);
            
            UpdateSlots(_inventoryItemSlots, inventory);
            UpdateSlots(_materialItemSlots, material);
        }

        #region FunctionsForUpdateSlotUI
        private void ChangeEquipmentItem()
        {
            foreach (var equipmentItem in _equipmentItemSlots)
            {
                foreach (var item 
                         in _equipmentDictionary.Where(item 
                             => item.Key.equipmentType == equipmentItem.slotType)) 
                    equipmentItem.UpdateSlot(item.Value);
            }
        }

        private static void CleanUpSlots(IEnumerable<UIItemSlot> slots)
        {
            foreach (var slot in slots) 
                slot.CleanUpSlot();
        }
        
        private static void UpdateSlots(IReadOnlyList<UIItemSlot> slots, IReadOnlyList<InventoryItem> items)
        {
            for (var i = 0; i < items.Count; i++) 
                slots[i].UpdateSlot(items[i]);
        }
        #endregion

        #region AddItemToInventory
        public void AddItem(ItemData newItemData)
        {
            var collection = newItemData.itemType == ItemType.Equipment ? _inventoryDictionary : _materialDictionary;
            var list = newItemData.itemType == ItemType.Equipment ? inventory : material;
            AddToCollection(newItemData, collection, list);

            #region OldCode
            /*switch (newItemData.itemType)
            {
                case ItemType.Equipment:
                    AddToInventory(newItemData);
                    break;
                case ItemType.Material:
                    AddToMaterial(newItemData);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }*/
            #endregion
            UpdateSlotUI();
        }

        private static void AddToCollection(ItemData newItemData, IDictionary<ItemData, InventoryItem> collection,
            ICollection<InventoryItem> list)
        {
            if(collection.TryGetValue(newItemData, out var existingItem))
                existingItem.AddStack();
            else
            {
                var newItem = new InventoryItem(newItemData);
                list.Add(newItem);
                collection.Add(newItemData, newItem);
            }
        }

        #region OldCodeForAddToCollection 
        /*private void AddToMaterial(ItemData newItemData)
        {
            if(_materialDictionary.TryGetValue(newItemData, out var existingItem))
                existingItem.AddStack();
            else
            {
                var newItem = new InventoryItem(newItemData);
                material.Add(newItem);
                _materialDictionary.Add(newItemData, newItem);
            }
        }

        private void AddToInventory(ItemData newItemData)
        {
            // If item already exists in inventory, add stack
            if (_inventoryDictionary.TryGetValue(newItemData, out var existingItem))
                existingItem.AddStack();
            // If item does not exist in inventory, create new item
            else
            {
                // Create new item and add to inventory
                var newItem = new InventoryItem(newItemData);
                inventory.Add(newItem);
                _inventoryDictionary.Add(newItemData, newItem);
            }
        }*/
        

        #endregion
        #endregion

        #region RemoveItemFromInventory
        private void RemoveItem(ItemData itemData)
        {
            var collection = itemData.itemType == ItemType.Equipment ? _inventoryDictionary : _materialDictionary;
            var list = itemData.itemType == ItemType.Equipment ? inventory : material;
            RemoveFromCollection(itemData, collection, list);
            
            #region OldCodeForRemoveFromCollection
            /*// If item exists in inventory, remove stack
            if (_inventoryDictionary.TryGetValue(itemData, out InventoryItem existingItem))
            {
                // If stack size is 1, remove item from inventory
                if (existingItem.stackSize <= 1)
                {
                    inventory.Remove(existingItem);
                    _inventoryDictionary.Remove(itemData);
                }
                // If stack size is greater than 1, remove stack
                else existingItem.RemoveStack();
            }
            
            if(_materialDictionary.TryGetValue(itemData, out InventoryItem existingMaterial))
            {
                if (existingMaterial.stackSize <= 1)
                {
                    material.Remove(existingMaterial);
                    _materialDictionary.Remove(itemData);
                }
                else existingMaterial.RemoveStack();
            }*/
            #endregion
            UpdateSlotUI();
        }
        
        private static void RemoveFromCollection(ItemData itemData, IDictionary<ItemData, InventoryItem> collection,
            ICollection<InventoryItem> list)
        {
            if (collection.TryGetValue(itemData, out var existingItem))
            {
                if (existingItem.stackSize <= 1)
                {
                    list.Remove(existingItem);
                    collection.Remove(itemData);
                }
                else existingItem.RemoveStack();
            }
        }
        #endregion

        public bool CanCraft(ItemDataEquipment itemToCraft, List<InventoryItem> requiredMaterials)
        {
            List<InventoryItem> materialsToRemove = new List<InventoryItem>();
            
            for (int i = 0; i < requiredMaterials.Count; i++)
            {
                if(_materialDictionary.TryGetValue(requiredMaterials[i].data, out var materialItem))
                {
                    if (materialItem.stackSize < requiredMaterials[i].stackSize)
                        return false;
                    materialsToRemove.Add(materialItem);
                }
                else return false;
            }

            for (int i = 0; i < materialsToRemove.Count; i++)
            {
                RemoveItem(materialsToRemove[i].data);
            }
            
            AddItem(itemToCraft);
            return true;
        }
    }
}
