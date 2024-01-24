using UnityEngine;

namespace Enemy.EnemyState.StateData
{
    [CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/Melee Attack State")]
    public class D_MeleeAttackState : ScriptableObject
    {
        public int comboTotal;
    }
}