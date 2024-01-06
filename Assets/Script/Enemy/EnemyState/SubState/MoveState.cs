using _Scripts.Enemies.EnemyState.StateData;
using Script.CoreSystem.CoreComponents;
using UnityEngine;


namespace Script.Enemy.EnemyState.SubState
{
    public class MoveState : EnemyStateMachine.EnemyState
    {
        private float _moveTime;
        protected bool IsDetectingWall;
        protected bool IsDetectingLedge;
        protected bool IsMoveTimeOver;
        protected bool IsPlayerInAgroRange;

        private readonly D_MoveState _stateData;

        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
            : Core.GetCoreComponent(ref _collisionSenses);

        private Movement _movement;
        private CollisionSenses _collisionSenses;
        
        protected MoveState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_MoveState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        protected override void DoChecks()
        {
            base.DoChecks();
            
            IsDetectingLedge = CollisionSenses.LedgeVertical;
            IsDetectingWall = CollisionSenses.WallFront;
            IsPlayerInAgroRange = EnemyBase.CheckPlayerInAgroRange();
        }

        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityX(_stateData.movementSpeed * Movement.FacingDirection);
            IsMoveTimeOver = false;
            SetRandomMoveTime();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (Time.time >= StartTime + _moveTime) 
                IsMoveTimeOver = true;
            Movement?.SetVelocityX(_stateData.movementSpeed * Movement.FacingDirection);
        }
        
        private void SetRandomMoveTime() => 
            _moveTime = Random.Range(_stateData.minMoveTime, _stateData.maxMoveTime);
    }
}