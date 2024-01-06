using _Scripts.Enemies.EnemyState.StateData;
using Script.Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Script.Enemy.EnemyState.SubState
{
    public class PlayerDetectedState : GroundedState
    {
        protected bool PerformLongRangeAction;
        private readonly D_PlayerDetectedState _stateData;


        protected PlayerDetectedState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_PlayerDetectedState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        
        public override void Enter()
        {
            base.Enter();

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