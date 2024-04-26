using StatSystem;
using UnityEngine;

namespace Controller
{
    public class ThunderTransitController : MonoBehaviour
    {
        [SerializeField] private CharacterStats targetStats;
        [SerializeField] private float speed;

        private Animator _animator;
        private bool _triggered;
        private static readonly int ThunderTransitHit = Animator.StringToHash("Hit");

        private void Start() => _animator = GetComponentInChildren<Animator>();

        public void Setup(CharacterStats targetGetHitStats) => targetStats = targetGetHitStats;

        private void Update()
        {
            if(!targetStats)
                return;
            if (_triggered)
                return;
            
            transform.position = Vector2.MoveTowards(transform.position, 
                targetStats.transform.position, speed * Time.deltaTime);
            
            transform.right = transform.position - targetStats.transform.position;

            if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
            {
                _animator.transform.localPosition = new Vector3(0, .5f);
                _animator.transform.localRotation = Quaternion.identity;

                transform.localRotation = Quaternion.identity;
                transform.localScale = new Vector3(3, 3);

                Invoke(nameof(DamageAndSelfDestroy), .2f);
                _triggered = true;
                _animator.SetTrigger(ThunderTransitHit);
            }
        }
        
        private void DamageAndSelfDestroy()
        {
            targetStats.ApplyShock(true);
            targetStats.TakeDamage(1);
            Destroy(gameObject, .4f);
        }
    }
}
