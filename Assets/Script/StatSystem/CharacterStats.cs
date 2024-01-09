using UnityEngine;

namespace Script.StatSystem
{
    public class CharacterStats : MonoBehaviour
    {
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
        
        
        
        [SerializeField] private int currentHealth;
        // Start is called before the first frame update
        protected virtual void Start()
        {
            critPower.SetDefaultValue(150);
            currentHealth = maxHealth.GetValue();
        }

        public virtual void DoDamage(CharacterStats targetStats)
        {
            if(TargetCanAvoidAttack(targetStats))
                return;
            var totalDamage = damage.GetValue() + strength.GetValue();

            if (CanCrit())
                totalDamage = CalculateCriticalDamage(totalDamage);
            
            totalDamage = CheckTargetArmor(targetStats, totalDamage);
            targetStats.TakeDamage(totalDamage);
        }


        // Update is called once per frame

        public virtual void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            Debug.Log(damageAmount);
            
            if (currentHealth <= 0) Die();
        }

        protected virtual void Die() {}
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
            int totalCriticalChance = critChance.GetValue() + agility.GetValue();
            return Random.Range(0, 100) <= totalCriticalChance;
        }
        private int CalculateCriticalDamage(int damageAmount)
        {
            var totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
            var critDamage = damageAmount * totalCritPower;

            return Mathf.RoundToInt(critDamage);
        }
    }
}
