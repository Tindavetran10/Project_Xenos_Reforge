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
        public static InventoryManager instance;
        
        public List<ItemData> startingEquipments;

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
        
        [Header("Items Cooldown")]
        private float _lastTimeUsedFlask;
        
        
        private void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            else instance = this;
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
            
            AddStartingItems();
        }
        
        private void AddStartingItems()
        {
            foreach (var startingEquipment in startingEquipments) 
                AddItem(startingEquipment);
        }

        public void EquipItem(ItemData itemData)
        {
            var newEquipment = itemData as ItemDataEquipment;
            var newItem = new InventoryItem(newEquipment);
            
            PreventPlayerEquipWeaponAlreadyEquipped(newEquipment);

            // Add new equipment (the same one) to the equipment list
            equipment.Add(newItem);
            
            // Add new equipment to equipment dictionary
            if (newEquipment != null)
            {
                _equipmentDictionary.Add(newEquipment, newItem);
                
                // Modify player stats when player equips equipment
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

            // Check if equipment of the same type is already equipped
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

            // If equipment of the same type is already equipped, remove the current equipment and add it to inventory
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
                // Remove modifiers from player stats when player unequipped equipment
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
            // Iterate through each UI equipment slot
            foreach (var equipmentItem in _equipmentItemSlots)
            {
                // For each slot, iterate through the equipment dictionary to find matching items by equipment type
                foreach (var item 
                         in _equipmentDictionary.Where(
                             item => item.Key.equipmentType == equipmentItem.slotType))
                    // Update the UI slot with the corresponding item if the equipment types match
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
            // Determine the appropriate collection and list based on the item type
            var collection = newItemData.itemType == EnumList.ItemType.Equipment ? _inventoryDictionary : _materialDictionary;
            var list = newItemData.itemType == EnumList.ItemType.Equipment ? inventory : material;
            // Add the new item to the determined collection and list
            AddToCollection(newItemData, collection, list);

            // Update the UI to reflect changes in the inventory or material list
            UpdateSlotUI();
        }

        private static void AddToCollection(ItemData newItemData, IDictionary<ItemData, InventoryItem> collection,
            ICollection<InventoryItem> list)
        {
            // Check if the item already exists in the collection
            if (collection.TryGetValue(newItemData, out var existingItem))
                // If the item exists, increase its stack size
                existingItem.AddStack();
            else
            {
                // If the item does not exist, create a new InventoryItem
                var newItem = new InventoryItem(newItemData);
                // Add the new item to the list
                list.Add(newItem);
                // Add the new item to the collection
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
        public void RemoveItem(ItemData itemData)
        {
            // Determine the appropriate collection and list based on the item's type
            var collection = itemData.itemType == EnumList.ItemType.Equipment ? _inventoryDictionary : _materialDictionary;
            var list = itemData.itemType == EnumList.ItemType.Equipment ? inventory : material;
    
            // Remove the item from the specified collection and list
            RemoveFromCollection(itemData, collection, list);
    
            // Update the UI to reflect the changes in the inventory or material list
            UpdateSlotUI();
        }

        private static void RemoveFromCollection(ItemData itemData, IDictionary<ItemData, InventoryItem> collection,
            ICollection<InventoryItem> list)
        {
            // Check if the item exists in the collection
            if (collection.TryGetValue(itemData, out var existingItem))
            {
                // If the item's stack size is 1, remove it completely from the list and collection
                if (existingItem.stackSize <= 1)
                {
                    list.Remove(existingItem);
                    collection.Remove(itemData);
                }
                // If the stack size is greater than 1, decrement the stack size by one
                else existingItem.RemoveStack();
            }
        }
        #endregion

        public bool CanCraft(ItemDataEquipment itemToCraft, List<InventoryItem> requiredMaterials)
        {
            var materialsToRemove = new List<InventoryItem>();
            
            foreach (var requiredMat in requiredMaterials)
            {
                if(_materialDictionary.TryGetValue(requiredMat.data, out var materialItem))
                {
                    if (materialItem.stackSize < requiredMat.stackSize)
                        return false;
                    materialsToRemove.Add(materialItem);
                }
                else return false;
            }

            foreach (var materials in materialsToRemove) 
                RemoveItem(materials.data);
            
            AddItem(itemToCraft);
            return true;
        }
        
        public IEnumerable<InventoryItem> GetEquipmentList() => equipment;
        public IEnumerable<InventoryItem> GetMaterialList() => material;
        
        public ItemDataEquipment GetEquipment(EnumList.EquipmentType equipmentType)
        {
            ItemDataEquipment equippedItem = null;
            
            foreach (var item 
                     in _equipmentDictionary.Where(
                         item => item.Key.equipmentType == equipmentType))
                equippedItem = item.Key;

            return equippedItem;
        }

        public void UseFlask()
        {
            ItemDataEquipment currentFlask = GetEquipment(EnumList.EquipmentType.Flask);
            
            if(currentFlask == null)
            {
                Debug.Log("No flask equipped");
                return;
            }
            
            bool canUseFlask = Time.time > _lastTimeUsedFlask + currentFlask.itemCoolDown;
            if (canUseFlask)
            {
                currentFlask.ExecuteItemEffect(null);
                _lastTimeUsedFlask = Time.time;
            }   
            else Debug.Log("Flask is on cooldown");
        }
    }
}
