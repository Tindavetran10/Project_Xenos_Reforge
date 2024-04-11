using StatSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHealthBar : MonoBehaviour
    {
        private global::Entity.Entity _entity;
        private RectTransform _myTransform;
        private Slider _slider;
        
        [SerializeField] private CharacterStats myStats;
        

        protected void Start()
        {
            _myTransform = GetComponent<RectTransform>();
            _entity = GetComponentInParent<global::Entity.Entity>();
            _slider = GetComponentInChildren<Slider>();

            _entity.OnFlipped += FlipUI;
            myStats.OnHealthChanged += UpdateHealthUI;
            
            UpdateHealthUI();
        }

        private void UpdateHealthUI()
        { 
            _slider.maxValue = myStats.GetMaxHealthValue();
            _slider.value = myStats.currentHealth;
        }

        private void FlipUI() => _myTransform.Rotate(0, 180, 0);
        private void OnDisable()
        {
            _entity.OnFlipped -= FlipUI;
            myStats.OnHealthChanged -= UpdateHealthUI;
        }
    }
}