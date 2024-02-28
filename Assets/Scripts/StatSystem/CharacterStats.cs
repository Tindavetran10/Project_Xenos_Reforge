using Entity;
using UnityEngine;

namespace StatSystem
{
    public class CharacterStats : MonoBehaviour
    {
        private EntityFX _fx;
        
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
        
        public System.Action OnHealthChanged;
        private bool IsDead { get; set; }

        protected bool IsInvincible { get; private set; }
        
        [SerializeField] public int currentHealth;
  
        protected virtual void Start()
        {
            critPower.SetDefaultValue(150);
            currentHealth = GetMaxHealthValue();

            _fx = GetComponentInParent<EntityFX>();
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
            
            if(!IsInvincible)
                _fx.StartCoroutine("FlashFX");
            
            if (currentHealth <= 0 && !IsDead) 
                Die();
        }

        private void DecreaseHealthBy(int damageAmount)
        {
            if (!IsInvincible)
                damageAmount = Mathf.RoundToInt(damageAmount * 1.1f);
            else damageAmount = 0;

            currentHealth -= damageAmount;
            OnHealthChanged?.Invoke();
        }

        protected virtual void Die() => IsDead = true;

        public void KillEntity()
        {
            if(!IsDead)
                Die();
        }

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

        public void MakeInvincible(bool invincible) => IsInvincible = invincible;
        #endregion
    }
}
