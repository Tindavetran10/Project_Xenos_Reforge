using System;
using InventorySystem_and_Items;
using Manager;
using StatSystem;
using UnityEngine;

namespace Enemy.EnemyStats
{
    public class EnemyStats : CharacterStats
    {
        private global::Enemy.EnemyStateMachine.Enemy _enemy;
        private ItemDrop _myDropSystem;
        
        [Space] public Stat energyDropAmount;
        
        [Header("Level details")]
        [SerializeField] private int level = 1;

        [Range(0f, 1f)] 
        [SerializeField] private float percentageModifier = 0.4f;
        
        protected override void Start()
        {
            energyDropAmount.SetDefaultValue(100);
            ApplyModifierBaseOnLevel();

            base.Start();
            
            _enemy = GetComponentInParent<Enemy.EnemyStateMachine.Enemy>();
            _myDropSystem = GetComponentInParent<ItemDrop>();
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

        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            if(_enemy!= null)
                _enemy.DamageImpact();
            else Debug.LogWarning("EnemyStats: _enemy is null when trying to apply damage impact.");
        }

        protected override void SetFlagDeath()
        {
            // From Character Stats: Set the flag IsDeath to true
            base.SetFlagDeath();
            
            // From the Entity: destroy the game object
            _enemy?.Die();
            
            PlayerManager.GetInstance().currency += energyDropAmount.GetValue();
            _myDropSystem?.GenerateDrop();
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

