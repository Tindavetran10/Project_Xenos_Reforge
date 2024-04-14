using InventorySystem_and_Items.Data;
using UnityEngine;

namespace InventorySystem_and_Items
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private ItemData itemData;
        
        private void SetupVisuals()
        {
            if (itemData == null)
                return;

            GetComponent<SpriteRenderer>().sprite = itemData.icon;
            gameObject.name = "Item object - " + itemData.itemName;
        }
        
        public void SetupItem(ItemData data, Vector2 velocity)
        {
            itemData = data;
            rb.velocity = velocity;

            SetupVisuals();
        }
        
        public void PickupItem()
        {
            InventoryManager.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
