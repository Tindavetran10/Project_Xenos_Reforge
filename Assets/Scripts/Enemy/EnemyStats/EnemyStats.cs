using Manager;
using StatSystem;
using UnityEngine;

namespace Enemy.EnemyStats
{
    public class EnemyStats : CharacterStats
    {
        private global::Enemy.EnemyStateMachine.Enemy _enemy;
        public Stat energyDropAmount;
        
        protected override void Start()
        {
            base.Start();
            energyDropAmount.SetDefaultValue(100);
            
            _enemy = GetComponentInParent<Enemy.EnemyStateMachine.Enemy>();
        }

        protected override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            if(_enemy!= null)
                _enemy.DamageImpact();
            else Debug.LogWarning("EnemyStats: _enemy is null when trying to apply damage impact.");
        }

        protected override void Die()
        {
            // From Character Stats
            base.Die();
            
            // From the Entity
            _enemy.Die();
            PlayerManager.GetInstance().currency += energyDropAmount.GetValue();
        }

        protected override void StunCloseRange()
        {
            base.StunCloseRange();
            _enemy.TryCloseCounterAttackWindow();
        }
        
        protected override void StunLongRange()
        {
            base.StunLongRange();
            _enemy.ChangeStunState();
        }
        
        protected override void Attacked()
        {
            base.Attacked();
            _enemy.ChangeGetAttackedState();
        }
    }
}

