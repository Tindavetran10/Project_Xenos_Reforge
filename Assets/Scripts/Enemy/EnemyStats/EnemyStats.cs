using Manager;
using StatSystem;

namespace Scripts.Enemy.EnemyStats
{
    public class EnemyStats : CharacterStats
    {
        private EnemyStateMachine.Enemy _enemy;
        public Stat soulDropAmount;
        
        protected override void Start()
        {
            soulDropAmount.SetDefaultValue(100);
            
            base.Start();
            _enemy = GetComponentInParent<EnemyStateMachine.Enemy>();
        }

        protected override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            _enemy.DamageImpact();
        }

        protected override void Die()
        {
            // From Character Stats
            base.Die();
            
            // From the Entity
            _enemy.Die();
            PlayerManager.Instance.currency += soulDropAmount.GetValue();
        }

        
    }
}

