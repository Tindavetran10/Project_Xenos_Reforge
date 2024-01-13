using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Script.Enemy.EnemyState.SubState
{
    public class DodgeState : BattleState
    {
        protected bool IsDodgeOver;

        private readonly D_DodgeState StateData;
        
        protected DodgeState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_DodgeState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            StateData = stateData;

        public override void Enter()
        {
            base.Enter();
            IsDodgeOver = false;
            Movement?.SetVelocity(StateData.dodgeSpeed, StateData.dodgeAngle, -Movement.FacingDirection);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (Time.time >= StartTime + StateData.dodgeTime && IsGrounded)
                IsDodgeOver = true;
        }
    }
}