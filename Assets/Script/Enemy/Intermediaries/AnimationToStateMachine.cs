using Script.Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Script.Enemy.Intermediaries
{
    public class AnimationToStateMachine : MonoBehaviour
    {
        public BattleState BattleState;
        
        private void FinishAttack() => BattleState.FinishAttack();
    }
}
