using Script.Enemy.EnemyStats;
using Script.StatSystem;
using UnityEngine;

namespace Script.Player.Skills.SkillController
{
    public class SlashSkillController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private CharacterStats _stats;
        private PlayerStateMachine.Player _player;
        [SerializeField] private float radius = 10f;
        
        
        private void Awake()
        {
            GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            GetComponent<BoxCollider2D>();
            _stats = GetComponentInChildren<CharacterStats>();
        }

        public void SetupSlash(Vector2 dir, float gravityScale)
        {
            _rb.velocity = dir;
            _rb.gravityScale = gravityScale;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var collider2Ds = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (var hit in collider2Ds)
            {
                if (hit.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null)
                {
                    var target = hit.GetComponentInChildren<EnemyStats>();
                    _stats.DoDamage(target);
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            // Draw a wire sphere in the Scene view to visualize the radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
