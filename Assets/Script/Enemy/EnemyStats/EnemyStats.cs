using Script.StatSystem;

namespace Script.Enemy.EnemyStats
{
    public class EnemyStats : CharacterStats
    {
        private EnemyStateMachine.Enemy _enemy;
        
        protected override void Start()
        {
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
            base.Die();
            _enemy.Die();
        }
    }
}

