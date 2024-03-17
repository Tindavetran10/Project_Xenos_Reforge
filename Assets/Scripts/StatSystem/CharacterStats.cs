using System;
using Entity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StatSystem
{
    public class CharacterStats : MonoBehaviour
    {
        private EntityFX _fx;

        #region Stat System
        [Header("Major stats")]
        public Stat strength; // 1 point increase damage and critical power by 1%
        public Stat agility; // 1 point increase evasion and critical chance by 1%
        public Stat intelligence; // 1 point increase magic damage and 1 magic resistance by 3%
        public Stat vitality; // 1 point increase health by 5%
 
        [Header("Offensive stats")]
        public Stat damage;
        public Stat critChance;
        public Stat critPower;
        
        [Header("Defensive stats")]
        public Stat maxHealth;
        public Stat armor;
        public Stat evasion;
        public Stat maxPoiseResistance;
        public Stat poiseResetTime;
        public Stat lastPoiseReset;
        #endregion
        
        // Used to change the health bar value
        public Action OnHealthChanged;
        
        public bool IsDead { get; private set; }
        public bool IsStunned { get; set; }
        public bool IsAttacked { get; set; }

        private bool IsInvincible { get; set; }
        
        [SerializeField] public int currentHealth;
        [SerializeField] public int currentPoise;
  
        protected virtual void Start()
        {
            critPower.SetDefaultValue(150);
            currentHealth = GetMaxHealthValue();
            currentPoise = GetMaxPoiseValue();

            _fx = GetComponentInParent<EntityFX>();
        }

        protected void Update()
        {
            if (Time.time >= lastPoiseReset.GetValue() + poiseResetTime.GetValue())
            {
                lastPoiseReset.SetDefaultValue((int) Time.time);
                currentPoise = GetMaxPoiseValue();
            }
        }

        public void DoDamage(CharacterStats targetStats)
        {
            if(TargetCanAvoidAttack(targetStats))
                return;
            var totalDamage = damage.GetValue() + strength.GetValue();

            if (CanCrit())
                totalDamage = CalculateCriticalDamage(totalDamage);
            
            totalDamage = CheckTargetArmor(targetStats, totalDamage);
            targetStats.TakeDamage(totalDamage);
        }
        
        protected virtual void TakeDamage(int damageAmount)
        {
            DecreaseHealthBy(damageAmount);
            DecreasePoiseBy(damageAmount);
            
            if(!IsInvincible)
            {
                _fx.StartCoroutine("FlashFX");
                Attacked();
            }

            if (currentHealth <= 0 && !IsDead) Die();
            if (currentPoise <= 0) Stun();
        }
        protected virtual void Stun() => IsStunned = true;
        protected virtual void Attacked() => IsAttacked = true;

        #region Main Calculations for Health and Poise
        private int CalculateAdjustedAmount(int amount) => 
            IsInvincible ? 0 : Mathf.RoundToInt(amount * 1.1f);

        private void DecreaseHealthBy(int damageAmount)
        {
            var adjustedHealth = CalculateAdjustedAmount(damageAmount);
            
            if (adjustedHealth > 0)
            {
                currentHealth -= adjustedHealth;
                OnHealthChanged?.Invoke();
            }
        }
        
        private void DecreasePoiseBy(int poiseAmount)
        {
            var adjustedPoise = CalculateAdjustedAmount(poiseAmount);
            if (adjustedPoise > 0) currentPoise -= adjustedPoise;
        }
        #endregion
        
        #region Make an Entity Die
        protected virtual void Die() => IsDead = true;

        public void KillEntity()
        {
            if(!IsDead)
                Die();
        }
        #endregion
        
        // No need to care if we finish the inventory system
        #region Stat Calculations
        private static int CheckTargetArmor(CharacterStats targetStats, int totalDamage)
        {
            totalDamage -= targetStats.armor.GetValue();
            totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
            return totalDamage;
        }
        private static bool TargetCanAvoidAttack(CharacterStats targetStats)
        {
            var totalEvasion = targetStats.evasion.GetValue() + targetStats.agility.GetValue();
            return Random.Range(0, 100) < totalEvasion;
        }
        private bool CanCrit()
        {
            var totalCriticalChance = critChance.GetValue() + agility.GetValue();
            return Random.Range(0, 100) <= totalCriticalChance;
        }
        private int CalculateCriticalDamage(int damageAmount)
        {
            var totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
            var critDamage = damageAmount * totalCritPower;

            return Mathf.RoundToInt(critDamage);
        }
        
        public int GetMaxHealthValue() => 
            maxHealth.GetValue() + vitality.GetValue() * 5;

        private int GetMaxPoiseValue() => maxPoiseResistance.GetValue();

        public void MakeInvincible(bool invincible) => IsInvincible = invincible;
        #endregion
    }
}
