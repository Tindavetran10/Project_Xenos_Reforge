using Enemy.EnemyState.StateData;
using Scripts.Enemy.EnemyState.SuperState;
using UnityEngine;


namespace Scripts.Enemy.EnemyState.SubState
{
    public class MoveState : GroundedState
    {
        private float _moveTime;
        protected bool IsMoveTimeOver;
        
        private readonly D_MoveState _stateData;
        
        protected MoveState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_MoveState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;
        
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