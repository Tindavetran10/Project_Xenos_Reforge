using UnityEngine;

namespace Enemy.EnemyState.StateData
{
    [CreateAssetMenu(fileName = "newRangedAttackStateData", menuName = "Data/State Data/Ranged Attack State")]
    public class D_RangedAttackState : ScriptableObject
    {
        public float rangedAttackCoolDown;
    }
}