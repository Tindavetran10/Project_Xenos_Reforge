using _Scripts.Enemies.EnemyState.StateData;
using Script.CoreSystem.CoreComponents;
using UnityEngine;

namespace Script.Enemy.EnemyState.SubState
{
    public class IdleState : EnemyStateMachine.EnemyState
    {
        private float _idleTime;
        private bool _flipAfterIdle;
        protected bool IsIdleTimeOver;

        protected bool IsPlayerInAgroRange;

        private readonly D_IdleState StateData;

        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;

        private CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
            : Core.GetCoreComponent(ref _collisionSenses);
        private CollisionSenses _collisionSenses;
        
        protected IdleState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_IdleState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            StateData = stateData;

        protected override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInAgroRange = EnemyBase.CheckPlayerInAgroRange();
        }

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
            _idleTime = Random.Range(StateData.minIdleTime, StateData.maxIdleTime);
    }
}