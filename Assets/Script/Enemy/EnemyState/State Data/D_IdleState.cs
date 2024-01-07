using UnityEngine;

namespace Script.Enemy.EnemyState.State_Data
{
    [CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/State Data/Idle State")]
    public class D_IdleState : ScriptableObject
    {
        [Header("Random Time Range")]
        public float minIdleTime = 1f;
        public float maxIdleTime = 2f;
    }
}