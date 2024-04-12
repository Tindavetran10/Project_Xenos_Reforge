using System.Collections.Generic;
using InventorySystem_and_Items.Data;
using Manager;
using UnityEngine;

namespace InventorySystem_and_Items
{
    public class ItemDrop : MonoBehaviour
    {
        [SerializeField] private int possibleItemsDrop;
        [SerializeField] private ItemData[] possibleDrops;
        private readonly List<ItemData> _dropList = new();
        
        [SerializeField] private GameObject dropPrefab;

        public void GenerateDrop()
        {
            if (_dropList.Count <= 0)
                return;

            foreach (var t in possibleDrops)
            {
                if (Random.Range(0, 100) <= t.dropChance) 
                    _dropList.Add(t);
            }

            for (var i = 0; i < possibleItemsDrop; i++)
            {
                var randomItem = _dropList[Random.Range(0, _dropList.Count - 1)];
                
                _dropList.Remove(randomItem);
                DropItem(randomItem);
            }
        }

        private void DropItem(ItemData itemData)
        {
            var newDrop = ObjectPoolManager.SpawnObject(dropPrefab, transform.position, Quaternion.identity);
            var randomVelocity = new Vector2(Random.Range(-5,5), Random.Range(12, 15));
            
            newDrop.GetComponent<ItemObject>().SetupItem(itemData, randomVelocity);
        }
    }
}