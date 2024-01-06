using _Scripts.Enemies.EnemyState.StateData;
using Script.Enemy.EnemyState.SuperState;
using UnityEngine;


namespace Script.Enemy.EnemyState.SubState
{
    public class MoveState : GroundedState
    {
        private float _moveTime;
        protected bool IsMoveTimeOver;
        protected bool IsDetectingWall;
        protected bool IsDetectingLedge;

        private readonly D_MoveState _stateData;

        
        protected MoveState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_MoveState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        protected override void DoChecks()
        {
            base.DoChecks();
            IsDetectingLedge = CollisionSenses.LedgeVertical;
            IsDetectingWall = CollisionSenses.WallFront;
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