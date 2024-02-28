using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Enemy.EnemyState.SubState
{
    public class DodgeState : BattleState
    {
        protected bool IsDodgeOver;

        private readonly D_DodgeState _stateData;
        
        protected DodgeState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
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