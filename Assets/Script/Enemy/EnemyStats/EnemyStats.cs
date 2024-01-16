using Script.Manager;
using Script.StatSystem;

namespace Script.Enemy.EnemyStats
{
    public class EnemyStats : CharacterStats
    {
        private EnemyStateMachine.Enemy _enemy;
        public Stat soulDropAmount;
        
        protected override void Start()
        {
            base.Start();
            soulDropAmount.SetDefaultValue(100);
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

            PlayerManager.Instance.currency += soulDropAmount.GetValue();
        }
    }
}

