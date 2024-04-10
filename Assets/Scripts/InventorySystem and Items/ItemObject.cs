using InventorySystem_and_Items.Data;
using UnityEngine;

namespace InventorySystem_and_Items
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;

        private void OnValidate()
        {
            if (itemData == null)
                return;
            
            GetComponent<SpriteRenderer>().sprite = itemData.icon;
            gameObject.name = "Item object - " + itemData.itemName;
        }
        
        /*private void SetupVisuals()
        {
            if (itemData == null)
                return;

            GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
            gameObject.name = "Item object - " + itemData.itemName;
        }
        
        public void SetupItem(ItemData _itemData, Vector2 _velocity)
        {
            itemData = _itemData;
            rb.velocity = _velocity;

            SetupVisuals();
        }*/
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player.PlayerStateMachine.Player>() != null)
            {
                InventoryManager.Instance.AddItem(itemData);
                Destroy(gameObject);
            }
        }
    }
}
