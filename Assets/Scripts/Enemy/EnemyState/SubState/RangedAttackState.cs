using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Enemy.EnemyState.SubState
{
    public class RangedAttackState : BattleState
    {
        private readonly D_RangedAttackState _stateData;
        
        protected RangedAttackState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_RangedAttackState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        public override void Enter()
        {
            base.Enter();
            EnemyBase.isAnimationFinished = false;
            EnemyBase.SpecialAttackTrigger();
        }
    }
}