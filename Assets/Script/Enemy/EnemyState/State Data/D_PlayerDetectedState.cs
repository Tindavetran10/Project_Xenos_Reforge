using UnityEngine;

namespace _Scripts.Enemies.EnemyState.StateData
{
    [CreateAssetMenu(fileName = "newPlayerDetectedStateData", menuName = "Data/State Data/Player Detected State")]
    public class D_PlayerDetectedState : ScriptableObject
    {
        public float longRangeActionTime = 1f;
    }
}