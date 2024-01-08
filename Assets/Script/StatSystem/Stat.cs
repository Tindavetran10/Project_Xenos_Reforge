using UnityEngine;

namespace Script.StatSystem
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField] private int baseValue;
        public int GetValue() => baseValue;
    }
}