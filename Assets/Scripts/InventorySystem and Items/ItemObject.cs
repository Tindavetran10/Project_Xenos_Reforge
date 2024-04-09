using InventorySystem_and_Items.Data;
using UnityEngine;

namespace InventorySystem_and_Items
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;

        private void OnValidate()
        {
            GetComponent<SpriteRenderer>().sprite = itemData.icon;
            gameObject.name = "Item object - " + itemData.itemName;
        }
        
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
