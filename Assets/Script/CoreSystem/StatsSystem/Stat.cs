using System;
using UnityEngine;

// A base system for lots of stat in game like Health, Magic, Stamina,...
namespace Script.CoreSystem.StatsSystem
{
    [Serializable]
    public class Stat
    {
        public event Action OnCurrentValueZero;
        [field: SerializeField] public float MaxValue { get; private set; }

        public float CurrentValue
        {
            // Get the current percent stat
            get => _currentValue;
            private set
            {
                _currentValue = Mathf.Clamp(value, 0f, MaxValue);
                // If stat reach down to 0, start OnCurrentValueZero event
                if (_currentValue <= 0) OnCurrentValueZero?.Invoke();
            }
        }

        private float _currentValue;

        
        // Create a stat with 100 percent
        public void Init() => CurrentValue = MaxValue;

        // Increase stat or decrease stat
        public void Increase(float amount) => CurrentValue += amount;
        public void Decrease(float amount) => CurrentValue -= amount;
    }
}