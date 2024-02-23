using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Enemy.EnemyState.SubState
{
    public class PlayerDetectedState : BattleState
    {
        private readonly D_PlayerDetectedState _stateData;
        
        protected PlayerDetectedState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_PlayerDetectedState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        
        public override void Enter()
        {
            base.Enter();
            EnemyBase.CloseCounterAttackWindow();
            PerformLongRangeAction = false;
            Movement?.SetVelocityX(0f);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            Movement?.SetVelocityX(0f);

            if (Time.time >= StartTime + _stateData.longRangeActionTime)
                PerformLongRangeAction = true;
        }
    }
}