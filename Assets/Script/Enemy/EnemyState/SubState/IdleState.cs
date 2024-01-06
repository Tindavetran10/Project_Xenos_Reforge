using _Scripts.Enemies.EnemyState.StateData;
using Script.Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Script.Enemy.EnemyState.SubState
{
    public class IdleState : GroundedState
    {
        private float _idleTime;
        private bool _flipAfterIdle;
        protected bool IsIdleTimeOver;
        
        private readonly D_IdleState _stateData;
        
        protected IdleState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_IdleState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;
        
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityX(0f);
            IsIdleTimeOver = false;
            SetRandomIdleTime();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            Movement?.SetVelocityX(0f);
            if (Time.time >= StartTime + _idleTime) IsIdleTimeOver = true;
        }

        public override void Exit()
        {
            base.Exit();
            if(_flipAfterIdle) Movement?.Flip();
        }

        public void SetFlipAfterIdle(bool flip) => 
            _flipAfterIdle = flip;

        private void SetRandomIdleTime() => 
            _idleTime = Random.Range(_stateData.minIdleTime, _stateData.maxIdleTime);
    }
}