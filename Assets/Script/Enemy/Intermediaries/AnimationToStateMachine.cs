using Script.Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Script.Enemy.Intermediaries
{
    public class AnimationToStateMachine : MonoBehaviour
    {
        public BattleState BattleState;
        public EnemyStateMachine.Enemy enemy;
        
        private void FinishAttack() => BattleState.FinishAttack();
        private void AttackTrigger() => BattleState.AttackTrigger();

        private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
        private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
    }
}
