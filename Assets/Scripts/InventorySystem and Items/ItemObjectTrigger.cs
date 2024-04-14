using StatSystem;
using UnityEngine;

namespace InventorySystem_and_Items
{
    public class ItemObjectTrigger : MonoBehaviour
    {
        private ItemObject MyItemObject => GetComponentInParent<ItemObject>();
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player.PlayerStateMachine.Player>() != null)
            {
                if (collision.GetComponentInChildren<CharacterStats>().IsDead)
                    return;
                MyItemObject.PickupItem();
            }
        }
    }
}
