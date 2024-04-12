using UnityEngine;

namespace InventorySystem_and_Items
{
    public class ItemObjectTrigger : MonoBehaviour
    {
        private ItemObject myItemObject => GetComponentInParent<ItemObject>();
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player.PlayerStateMachine.Player>() != null)
            {
                myItemObject.PickupItem();
            }
        }
    }
}
