using UnityEngine;

namespace InventorySystem_and_Items
{
    public class ItemObject : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        
        [SerializeField] private ItemData itemData;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = itemData.Icon;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Player.PlayerStateMachine.Player>() != null)
            {
                Debug.Log("Picked up Item" + itemData.ItemName);
                Destroy(gameObject);
            }
        }
    }
}
