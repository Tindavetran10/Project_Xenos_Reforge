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

        
        [Header("Soul Info")]
        [SerializeField] private TextMeshProUGUI currentSouls;
        [SerializeField] private float soulsAmount;
        [SerializeField] private float increaseRate = 100;
        
        private void Start()
        {
            if (playerStats != null)
                playerStats.OnHealthChanged += UpdateHealthUI;
        }

        private void Update() => UpdateSoulsUI();

        private void UpdateSoulsUI()
        {
            if (soulsAmount < PlayerManager.Instance.GetCurrency())
                soulsAmount += Time.deltaTime * increaseRate;
            else soulsAmount = PlayerManager.Instance.GetCurrency();

            currentSouls.text = ((int) soulsAmount).ToString();
        }

        private void UpdateHealthUI()
        {
            slider.maxValue = playerStats.GetMaxHealthValue();
            slider.value = playerStats.currentHealth;
        }
    }
}