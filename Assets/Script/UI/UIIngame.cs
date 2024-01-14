using Script.Manager;
using Script.Player.PlayerStats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class UIIngame : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private Slider slider;

        [SerializeField] private TextMeshProUGUI currentSouls;
        
        private void Start()
        {
            if (playerStats != null)
                playerStats.OnHealthChanged += UpdateHealthUI;
        }

        private void Update()
        {
            currentSouls.text = PlayerManager.Instance.GetCurrency().ToString("#,#");
        }

        private void UpdateHealthUI()
        {
            slider.maxValue = playerStats.GetMaxHealthValue();
            slider.value = playerStats.currentHealth;
        }
    }
}