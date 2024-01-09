using UnityEngine;

namespace Script.StatSystem
{
    public class CharacterStats : MonoBehaviour
    {
        public Stat strength;
        public Stat damage;
        public Stat maxHealth;

        [SerializeField] private int currentHealth;
        // Start is called before the first frame update
        protected virtual void Start() => 
            currentHealth = maxHealth.GetValue();

        public virtual void DoDamage(CharacterStats targetStats)
        {
            var totalDamage = damage.GetValue() + strength.GetValue();
            targetStats.TakeDamage(totalDamage);
        }
        
        // Update is called once per frame
        public virtual void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            Debug.Log(damageAmount);
            
            if (currentHealth <= 0) Die();
        }

        public virtual void Die()
        {
            
        }
    }
}
