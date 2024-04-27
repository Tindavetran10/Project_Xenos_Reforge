using System.Collections;
using HitStop;
using Manager;
using StatSystem;
using UnityEngine;

namespace Controller
{
    public class ProjectileController : MonoBehaviour
    {
        //[SerializeField] private int damage;
        [SerializeField] private string targetLayerName = "Player";

        [SerializeField] private float xVelocity;
        [SerializeField] private Rigidbody2D rb;
        private CapsuleCollider2D _projectileCollider2D;

        [SerializeField] private bool canMove;
        [SerializeField] private bool flipped;

        private HitStopController _hitStopController;
        [SerializeField] private float hitStopDuration;

        private int _targetLayer;
        private int _collisionLayer;
        private int _groundLayer;
        private CharacterStats _characterStats;
        
        private Coroutine _returnToPoolTimerCoroutine;

        private void OnEnable()
        {
            if (_returnToPoolTimerCoroutine != null)
                StopCoroutine(_returnToPoolTimerCoroutine);
            
            _returnToPoolTimerCoroutine = StartCoroutine(ReturnToPoolAfterTime());
            
            ResetProjectilePropertiesWhenTakingFromPool();
        }

        private void ResetProjectilePropertiesWhenTakingFromPool()
        {
            // Reset the canMove flag and the flipped flag to default values
            canMove = true;
            flipped = false;
            
            // Reset the target layer to the default value to "Player"
            targetLayerName = "Player";
            _targetLayer = LayerMask.NameToLayer(targetLayerName);
            
            // Reset the projectile collider and the rigidbody to their default values
            if(_projectileCollider2D != null)
                _projectileCollider2D.enabled = true;
            
            if(rb != null)
            {
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }

        private void Awake()
        {
            _hitStopController = HitStopController.Instance;
            _targetLayer = LayerMask.NameToLayer(targetLayerName);
            _groundLayer = LayerMask.NameToLayer("Ground");

            _projectileCollider2D = GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            if(canMove)
                rb.velocity = new Vector2(xVelocity, rb.velocity.y);
            _targetLayer = LayerMask.NameToLayer(targetLayerName);
        }
        
        private IEnumerator ReturnToPoolAfterTime()
        {
            var elapsedTime = 0f;
            
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        public void SetUpProjectile(float speed, CharacterStats characterStats)
        {
            xVelocity = speed;
            _characterStats = characterStats;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _collisionLayer = collision.gameObject.layer;

            // Decide if the projectile should interact with the game object it collided with base on the layer
            if (_collisionLayer == _targetLayer)
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
            else if(_collisionLayer == _groundLayer)
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
            if(_projectileCollider2D != null)
                _projectileCollider2D.enabled = false;
            
            canMove = false;
            StopProjectileMovement();
            
            ObjectPoolManager.ReturnObjectToPool(gameObject);
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
