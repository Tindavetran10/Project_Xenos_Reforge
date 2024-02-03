using Enemy.EnemyState.StateData;
using Scripts.Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Scripts.Enemy.EnemyState.SubState
{
    public class StunState : BattleState
    {
        private bool _isGrounded;
        protected bool IsStunTimeOver;
        private bool _isMovementStopped;

        private readonly D_StunState _stateData;
        
        protected StunState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_StunState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        protected override void DoChecks()
        {
            base.DoChecks();
            _isGrounded = CollisionSenses.Ground;
        }

        public override void Enter()
        {
            base.Enter();
            IsStunTimeOver = false;
            _isMovementStopped = false;
            Movement?.SetVelocity(-Movement.FacingDirection * _stateData.stunKnockbackSpeed, _stateData.stunKnockbackAngle);
            
            EnemyBase.FX.InvokeRepeating("RedColorBlink",0, .1f);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Time.time >= StartTime + _stateData.stunTime)
                IsStunTimeOver = true;

            if (_isGrounded && Time.time >= StartTime + _stateData.stunKnockbackTime && !_isMovementStopped)
            {
                _isMovementStopped = true;
                Movement?.SetVelocityX(0f);
            }
        }

        public override void Exit()
        {
            base.Exit();
            EnemyBase.FX.Invoke("CancelRedBlink", 0f);
        }
    }
}