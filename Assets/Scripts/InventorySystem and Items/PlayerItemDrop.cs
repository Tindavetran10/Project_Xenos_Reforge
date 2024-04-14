using System.Collections.Generic;
using System.Linq;
using InventorySystem_and_Items.Data;
using UnityEngine;

namespace InventorySystem_and_Items
{
    public class PlayerItemDrop : ItemDrop
    {
        [Header("Player's drop")]
        [SerializeField] private float chanceToLooseItems;
        [SerializeField] private float chanceToLooseMaterials;

        public override void GenerateDrop()
        {
            //base.GenerateDrop();
            var inventory = InventoryManager.instance;

            var itemsToUnequip = new List<InventoryItem>();
            var materialsToLoose = new List<InventoryItem>();

            foreach (var item in inventory.GetEquipmentList().Where(_ => Random.Range(0, 100) <= chanceToLooseItems))
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }

            foreach (var item in itemsToUnequip) 
                inventory.UnequipItem(item.data as ItemDataEquipment);
            
            foreach (var item in inventory.GetMaterialList().Where(_ => Random.Range(0, 100) <= chanceToLooseMaterials))
            {
                DropItem(item.data);
                materialsToLoose.Add(item);
            }

            foreach (var material in materialsToLoose) 
                inventory.RemoveItem(material.data);
        }
    }
}