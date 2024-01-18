using UnityEngine;

namespace Script.Intermediaries
{
    public class EnemyAnimationToStateMachine : MonoBehaviour
    {
        public Enemy.EnemyStateMachine.Enemy enemy;
        
        private void FinishAttack() => enemy.FinishAttack();
        private void AttackTrigger() => enemy.AttackTrigger();
        private void SpecialAttackTrigger() => enemy.SpecialAttackTrigger();

        private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
        private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
    }
}
