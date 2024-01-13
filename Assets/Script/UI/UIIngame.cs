using Script.Player.PlayerStats;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class UIIngame : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private Slider slider;

        private void Start()
        {
            if (playerStats != null)
                playerStats.OnHealthChanged += UpdateHealthUI;
        }
        
        private void UpdateHealthUI()
        {
            slider.maxValue = playerStats.GetMaxHealthValue();
            slider.value = playerStats.currentHealth;
        }
    }
}