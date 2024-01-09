using Script.StatSystem;
using UnityEngine;

namespace Script.Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private string targetLayerName = "Player";

        [SerializeField] private float xVelocity;
        [SerializeField] private Rigidbody2D rb;

        [SerializeField] private bool canMove;
        [SerializeField] private bool flipped;
        
        private void Update()
        {
            if(canMove)
                rb.velocity = new Vector2(xVelocity, rb.velocity.y);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
            {
                collision.GetComponentInChildren<CharacterStats>().TakeDamage(damage);
                ProjectileInteraction(collision);
            }
            else if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
                ProjectileInteraction(collision);
        }

        private void ProjectileInteraction(Collider2D collision)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            canMove = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.parent = collision.transform;
            
            Destroy(gameObject);
        }

        public void FlipArrow()
        {
            if(flipped) return;

            xVelocity *= -1;
            flipped = true;
            transform.Rotate(0, 180, 0);
            targetLayerName = "Enemy";
        }
    }
}
