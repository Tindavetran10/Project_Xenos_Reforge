using System.Collections.Generic;
using InventorySystem_and_Items.Data;
using UnityEngine;

namespace UI
{
    public class UICraftList : MonoBehaviour
    {
        [SerializeField] private Transform craftSlotParent;
        [SerializeField] private GameObject craftSlotPrefab;
        
        [SerializeField] private List<ItemDataEquipment> craftEquipmentList;
        [SerializeField] private List<UICraftSlot> craftSlots;
        
        private void Start() => AssignCraftSlot();

        private void AssignCraftSlot()
        {
            for (var i = 0; i < craftSlotParent.childCount; i++) 
                craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<UICraftSlot>());
        }

        public void SetupCraftList()
        {
            // Destroy old craftSlotPrefab objects
            foreach (var craftSlot in craftSlots) 
                Destroy(craftSlot.gameObject);

            // Clear the craftSlots list
            craftSlots.Clear();

            foreach (var craftEquipment in craftEquipmentList)
            {
                var newSlot = Instantiate(craftSlotPrefab, craftSlotParent);

                var newSlotRectTransform = newSlot.GetComponent<RectTransform>();
                var prefabRectTransform = craftSlotPrefab.GetComponent<RectTransform>();

                // Set the local position and local scale to match the original prefab
                newSlotRectTransform.localPosition = prefabRectTransform.localPosition;
                newSlotRectTransform.localScale = prefabRectTransform.localScale;

                var craftSlot = newSlot.GetComponent<UICraftSlot>();
                craftSlot.SetupCraftSlot(craftEquipment);

                // Add the new craftSlot to the craftSlots list
                craftSlots.Add(craftSlot);
            }
        }
    }
}