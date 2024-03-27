using HitStop;
using StatSystem;
using UnityEngine;

namespace Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        //[SerializeField] private int damage;
        [SerializeField] private string targetLayerName = "Player";

        [SerializeField] private float xVelocity;
        [SerializeField] private Rigidbody2D rb;
        private CapsuleCollider2D projectileCollider2D;

        [SerializeField] private bool canMove;
        [SerializeField] private bool flipped;

        private HitStopController _hitStopController;
        [SerializeField] private float hitStopDuration;

        private int targetLayer;
        private int groundLayer;
        private CharacterStats _characterStats;
        
        private void Start()
        {
            _hitStopController = HitStopController.Instance;
            targetLayer = LayerMask.NameToLayer(targetLayerName);
            groundLayer = LayerMask.NameToLayer("Ground");

            projectileCollider2D = GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            if(canMove)
                rb.velocity = new Vector2(xVelocity, rb.velocity.y);
            targetLayer = LayerMask.NameToLayer(targetLayerName);
        }

        public void SetUpProjectile(float speed, CharacterStats characterStats)
        {
            xVelocity = speed;
            _characterStats = characterStats;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var collisionLayer = collision.gameObject.layer;
            
            // Decide if the projectile should interact with the game object it collided with base on the layer
            if (collisionLayer == targetLayer)
            {
                var characterStats = collision.GetComponentInChildren<CharacterStats>();
                
                // If the projectile collided with a character, do damage and apply hit stop
                if (characterStats != null)
                {
                    _characterStats.DoDamage(characterStats);
                    _hitStopController.HitStop(hitStopDuration);
                }
                ProjectileInteraction();
            }
            else if(collisionLayer == groundLayer)
                ProjectileInteraction();
        }

        private void StopProjectileMovement()
        {
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        
        private void ProjectileInteraction()
        {
            if(projectileCollider2D != null)
                projectileCollider2D.enabled = false;
            
            canMove = false;
            StopProjectileMovement();
            
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
