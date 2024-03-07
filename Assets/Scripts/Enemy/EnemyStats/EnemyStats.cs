using Manager;
using StatSystem;

namespace Enemy.EnemyStats
{
    public class EnemyStats : CharacterStats
    {
        private global::Enemy.EnemyStateMachine.Enemy _enemy;
        public Stat soulDropAmount;
        
        protected override void Start()
        {
            soulDropAmount.SetDefaultValue(100);
            
            base.Start();
            _enemy = GetComponentInParent<Enemy.EnemyStateMachine.Enemy>();
        }

        protected override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            GetComponentInParent<Enemy.EnemyStateMachine.Enemy>().DamageImpact();
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

        protected override void Stun()
        {
            base.Stun();
            _enemy.CanBeStunned();
        }
    }
}

