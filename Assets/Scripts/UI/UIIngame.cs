using Manager;
using Player.PlayerStats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
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
            if (soulsAmount < PlayerManager.GetInstance().GetCurrency())
                soulsAmount += Time.deltaTime * increaseRate;
            else soulsAmount = PlayerManager.GetInstance().GetCurrency();

            currentSouls.text = ((int) soulsAmount).ToString();
        }

        private void UpdateHealthUI()
        {
            slider.maxValue = playerStats.GetMaxHealthValue();
            slider.value = playerStats.currentHealth;
        }
    }
}