using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Script.Enemy.EnemyState.SubState
{
    public class DodgeState : BattleState
    {
        protected bool IsDodgeOver;
        
        protected readonly D_DodgeState _stateData;
        
        protected DodgeState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_DodgeState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        public override void Enter()
        {
            base.Enter();
            IsDodgeOver = false;
            Movement?.SetVelocity(_stateData.dodgeSpeed, _stateData.dodgeAngle, -Movement.FacingDirection);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (Time.time >= StartTime + _stateData.dodgeTime && IsGrounded)
                IsDodgeOver = true;
        }
    }
}