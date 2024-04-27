using StatSystem;
using UnityEngine;

namespace Controller
{
    public class ThunderTransitController : MonoBehaviour
    {
        [SerializeField] private CharacterStats targetStats;
        [SerializeField] private float speed;
        [SerializeField] private int thunderTransitDamage;

        private Animator _animator;
        private bool _triggered;
        private static readonly int ThunderTransitHit = Animator.StringToHash("Hit");

        private void Start() => _animator = GetComponentInChildren<Animator>();

        public void Setup(int damage, CharacterStats targetGetHitStats)
        {
            thunderTransitDamage = damage;
            targetStats = targetGetHitStats;
        }

        private void Update()
        {
            if(!targetStats || _triggered)
                return;
            
            transform.position = Vector2.MoveTowards(transform.position, 
                targetStats.transform.position, speed * Time.deltaTime);
            
            transform.right = transform.position - targetStats.transform.position;

            // Check if the distance between the player and the target is less than .1f
            if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
            {
                // Invoke the DamageAndSelfDestroy method after .2f seconds
                Invoke(nameof(DamageAndSelfDestroy), .2f);
                _triggered = true;
                _animator.SetTrigger(ThunderTransitHit);
            }
        }
        
        private void DamageAndSelfDestroy()
        {
            targetStats.ApplyShock(true);
            targetStats.TakeDamage(thunderTransitDamage);
            Destroy(gameObject, .4f);
        }
    }
}
