using StatSystem;
using UnityEngine;

namespace Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private string targetLayerName = "Player";

        [SerializeField] private float xVelocity;
        [SerializeField] private Rigidbody2D rb;

        [SerializeField] private bool canMove;
        [SerializeField] private bool flipped;

        private CharacterStats _characterStats;
        
        private void Update()
        {
            if(canMove)
                rb.velocity = new Vector2(xVelocity, rb.velocity.y);
        }

        public void SetUpProjectile(float speed, CharacterStats characterStats)
        {
            xVelocity = speed;
            _characterStats = characterStats;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
            {
                _characterStats.DoDamage(collision.GetComponentInChildren<CharacterStats>());
                ProjectileInteraction(collision);
            }
            else if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
                ProjectileInteraction(collision);
        }

        private void ProjectileInteraction(Component collision)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            canMove = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.parent = collision.transform;
            
            Destroy(gameObject);
        }

        public void FlipProjectile()
        {
            if(flipped) return;

            xVelocity *= -1;
            flipped = true;
            transform.Rotate(0, 180, 0);
            targetLayerName = "Enemy";
        }
    }
}
