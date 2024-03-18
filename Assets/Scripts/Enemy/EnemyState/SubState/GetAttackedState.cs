using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Enemy.EnemyState.SubState
{
    public class GetAttackedState : BattleState
    {
        private bool _isGrounded;
        protected bool IsGetAttackedTimeOver;
        private bool _isMovementStopped;
        
        private readonly D_GetAttacked _stateData;
        
        protected GetAttackedState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine,
            string animBoolName, D_GetAttacked stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        
        protected override void DoChecks()
        {
            base.DoChecks();
            _isGrounded = CollisionSenses.Ground;
        }
        
        public override void Enter()
        {
            base.Enter();
            IsGetAttackedTimeOver = false;
            _isMovementStopped = false;
            Movement?.SetVelocity(-Movement.FacingDirection * _stateData.getAttackedKnockbackSpeed, _stateData.getAttackedKnockbackAngle);
            EnemyBase.Stats.IsAttacked = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (Time.time >= StartTime + _stateData.getAttackedTime) 
                IsGetAttackedTimeOver = true;
            
            if (_isGrounded && Time.time >= StartTime + _stateData.getAttackedKnockbackTime && !_isMovementStopped)
            {
                _isMovementStopped = true;
                Movement?.SetVelocityX(0f);
            }
            
        }
        
        public override void Exit()
        {
            base.Exit();
            EnemyBase.Stats.IsAttacked = false;
        }
    }
}