using HitStop;
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

        private HitStopController _hitStopController;
        [SerializeField] private float hitStopDuration;
        private CharacterStats _characterStats;
        
        private void Start() => _hitStopController = HitStopController.Instance;

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
            // Decide if the projectile should interact with the game object it collided with base on the layer
            if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
            {
                // If the projectile collided with a character, do damage and apply hit stop
                _characterStats.DoDamage(collision.GetComponentInChildren<CharacterStats>());
                _hitStopController.HitStop(hitStopDuration);
                ProjectileInteraction();
            }
            else if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
                ProjectileInteraction();
        }
        
        private void ProjectileInteraction()
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            canMove = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            
            Destroy(gameObject);
        }

        public void FlipProjectile()
        {
            // If the projectile is already flipped, return
            if(flipped) return;

            // Flip the projectile and change the target layer
            xVelocity *= -1;
            flipped = true;
            transform.Rotate(0, 180, 0);
            targetLayerName = "Enemy";
        }
    }
}
