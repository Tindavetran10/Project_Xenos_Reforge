using System.Collections.Generic;
using InventorySystem_and_Items.Data;
using Manager;
using UnityEngine;

namespace InventorySystem_and_Items
{
    public class ItemDrop : MonoBehaviour
    {
        [SerializeField] private int maxItemsToDrop;
        [SerializeField] private ItemData[] itemPool;
        private readonly List<ItemData> possibleDrop = new();
        
        [SerializeField] private GameObject dropPrefab;

        public void GenerateDrop()
        {
            if(itemPool.Length == 0) return;

            foreach (var itemData in itemPool)
            {
                if (itemPool != null && Random.Range(0, 100) < itemData.dropChance) 
                    possibleDrop.Add(itemData);
            }

            for (int i = 0; i < maxItemsToDrop; i++)
            {
                if (possibleDrop.Count > 0)
                {
                    int randomIndex = Random.Range(0, possibleDrop.Count);
                    ItemData itemToDrop = possibleDrop[randomIndex];
                    
                    DropItem(itemToDrop);
                    possibleDrop.Remove(itemToDrop);
                }
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