using UnityEngine;

namespace _Scripts.Enemies.EnemyState.StateData
{
    [CreateAssetMenu(fileName = "newMoveStateData", menuName = "Data/State Data/Move State")]
    public class D_MoveState : ScriptableObject
    {
        public float movementSpeed = 3f;

        public float minMoveTime;
        public float maxMoveTime;
    }
}