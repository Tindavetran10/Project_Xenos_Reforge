using UnityEngine;

namespace _Scripts.Enemies.EnemyState.StateData
{
    [CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/State Data/Idle State")]
    public class D_IdleState : ScriptableObject
    {
        [Header("Random Time Range")]
        public float minIdleTime = 1f;
        public float maxIdleTime = 2f;
    }
}