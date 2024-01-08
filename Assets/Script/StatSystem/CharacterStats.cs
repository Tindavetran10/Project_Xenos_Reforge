using UnityEngine;

namespace Script.StatSystem
{
    public class CharacterStats : MonoBehaviour
    {
        public Stat damage;
        public Stat maxHealth;

        [SerializeField] private int currentHealth;
        // Start is called before the first frame update
        private void Start()
        {
            currentHealth = maxHealth.GetValue();
        }

        // Update is called once per frame
        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            if (currentHealth <= 0) Die();
        }

        private void Die()
        {
            
        }
    }
}
