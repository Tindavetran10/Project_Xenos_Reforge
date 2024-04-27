using System.Collections.Generic;
using InventorySystem_and_Items.Data;
using Manager;
using UnityEngine;

namespace UI
{
    public class UICraftList : MonoBehaviour
    {
        [SerializeField] private Transform craftSlotParent;
        [SerializeField] private GameObject craftSlotPrefab;
        
        [SerializeField] private List<ItemDataEquipment> craftEquipmentList;
        
        private void Start()
        {
            transform.parent.GetChild(0).GetComponent<UICraftList>().SetupCraftList();
            SetupDefaultCraftWindow();
        }
        
        public void SetupCraftList()
        {
            // Destroy old craftSlotPrefab objects
            for (var i = 0; i < craftSlotParent.childCount; i++) 
                Destroy(craftSlotParent.GetChild(i).gameObject);
            

            foreach (var craftEquipment in craftEquipmentList)
            {
                var newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
                var craftSlot = newSlot.GetComponent<UICraftSlot>();
                craftSlot.SetupCraftSlot(craftEquipment);
            }
        }

        private void SetupDefaultCraftWindow()
        {
            if (craftEquipmentList[0] != null)
                GetComponentInParent<UIManager>().craftWindow.SetupCraftWindow(craftEquipmentList[0]);
        }
    }
}
