using UnityEngine;

namespace Script.Enemy.EnemyState.State_Data
{
    [CreateAssetMenu(fileName = "newMoveStateData", menuName = "Data/State Data/Move State")]
    public class D_MoveState : ScriptableObject
    {
        public float movementSpeed;
        
        [Header("Random Time Range")]
        public float minMoveTime;
        public float maxMoveTime;
    }
}