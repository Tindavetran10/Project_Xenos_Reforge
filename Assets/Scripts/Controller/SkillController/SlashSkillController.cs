using Enemy.EnemyStats;
using InventorySystem_and_Items;
using StatSystem;
using UnityEngine;

namespace Controller.SkillController
{
    public class SlashSkillController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private CharacterStats _stats;
        [SerializeField] private float radius = 10f;
        
        
        private void Awake()
        {
            //GetComponent<Animator>();
            //GetComponent<BoxCollider2D>();
            
            _rb = GetComponent<Rigidbody2D>();
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
                    
                    ExecuteWeaponEffectToSkill(hit);
                }
            }
        }

        private static void ExecuteWeaponEffectToSkill(Collider2D hit)
        {
            var equippedWeapon = InventoryManager.instance.GetEquipment(EnumList.EquipmentType.Weapon);

            if (equippedWeapon != null) 
                equippedWeapon.ExecuteItemEffect(hit.transform);
        }

        private void OnDrawGizmos()
        {
            // Draw a wire sphere in the Scene view to visualize the radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
