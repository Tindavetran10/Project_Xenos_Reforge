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
        private readonly List<ItemData> _possibleDrop = new();
        
        [SerializeField] private GameObject dropPrefab;

        public virtual void GenerateDrop()
        {
            if (itemPool.Length == 0)
            {
                Debug.Log("Item Pool is empty. Enemy cannot drop items.");
                return;
            }

            foreach (var itemData in itemPool)
            {
                if (itemPool != null && Random.Range(0, 100) <= itemData.dropChance) 
                    _possibleDrop.Add(itemData);
            }

            for (var i = 0; i < maxItemsToDrop; i++)
            {
                if (_possibleDrop.Count > 0)
                {
                    var randomIndex = Random.Range(0, _possibleDrop.Count);
                    var itemToDrop = _possibleDrop[randomIndex];
                    
                    DropItem(itemToDrop);
                    _possibleDrop.Remove(itemToDrop);
                }
            }
        }

        protected void DropItem(ItemData itemData)
        {
            var newDrop = ObjectPoolManager.SpawnObject(dropPrefab, transform.position, Quaternion.identity, 
                ObjectPoolManager.PoolType.GameObject);
            var randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
            
            newDrop.GetComponent<ItemObject>().SetupItem(itemData, randomVelocity);
        }
    }
}