using StatSystem;
using UnityEngine;

namespace MapLimit
{
    public class DeadZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponentInChildren<CharacterStats>() != null)
                collision.GetComponentInChildren<CharacterStats>().KillEntity();
            else Destroy(collision.gameObject);
        }
    }
}
