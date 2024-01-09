using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script.StatSystem
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField] private int baseValue;

        public List<int> modifiers;
        
        public int GetValue() => 
            baseValue + modifiers.Sum();

        public void SetDefaultValue(int value) => baseValue = value;
        public void AddModifier(int modifier) => modifiers.Add(modifier);
        public void RemoveModifier(int modifier) => modifiers.Remove(modifier);
    }
}