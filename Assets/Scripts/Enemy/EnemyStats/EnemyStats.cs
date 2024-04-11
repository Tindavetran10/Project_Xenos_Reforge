using System;
using Manager;
using StatSystem;
using UnityEngine;

namespace Enemy.EnemyStats
{
    public class EnemyStats : CharacterStats
    {
        private global::Enemy.EnemyStateMachine.Enemy _enemy;
        public Stat energyDropAmount;
        
        [Header("Level details")]
        [SerializeField] private int level = 1;

        [Range(0f, 1f)] 
        [SerializeField] private float percentageModifier = 0.4f;
        
        protected override void Start()
        {
            ApplyModifierBaseOnLevel();

            base.Start();
            energyDropAmount.SetDefaultValue(100);
            _enemy = GetComponentInParent<Enemy.EnemyStateMachine.Enemy>();
        }

        private void ApplyModifierBaseOnLevel()
        {
            Action<Stat> modifyStat = ModifyStats;
            Array.ForEach(new[] { strength, agility, intelligence, vitality, 
                damage, critChance, critPower, 
                maxHealth, armor, evasion, maxPoiseResistance, poiseResetTime, lastPoiseReset, energyDropAmount }, 
                modifyStat);
        }

        private void ModifyStats(Stat stat)
        {
            for(var i = 1; i < level; i++)
            {
                var modifier = stat.GetValue() * percentageModifier;
                stat.AddModifier(Mathf.RoundToInt(modifier));
            }
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

