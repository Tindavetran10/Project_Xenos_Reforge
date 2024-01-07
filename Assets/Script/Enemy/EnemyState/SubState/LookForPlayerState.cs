using Script.Enemy.EnemyState.State_Data;
using Script.Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Script.Enemy.EnemyState.SubState
{
    public class LookForPlayerState : GroundedState
    {
        private bool _turnImmediately;
        private bool _isAllTurnsDone;
        protected bool IsAllTurnsTimeDone;

        private float _lastTurnTime;
        private int _amountOfTurnsDone;
        
        private readonly D_LookForPlayerState _stateData;
        
        protected LookForPlayerState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_LookForPlayerState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;
        
        public override void Enter()
        {
            base.Enter();

            _isAllTurnsDone = false;
            IsAllTurnsTimeDone = false;

            _lastTurnTime = StartTime;
            _amountOfTurnsDone = 0;
            
            Movement?.SetVelocityX(0f);
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            Movement?.SetVelocityX(0f);

            if (_turnImmediately)
            {
                Movement?.Flip();
                _lastTurnTime = Time.time;
                _amountOfTurnsDone++;
                _turnImmediately = false;
            } 
            else if (Time.time >= _lastTurnTime + _stateData.timeBetweenTurns && !_isAllTurnsDone)
            {
                Movement?.Flip();
                _lastTurnTime = Time.time;
                _amountOfTurnsDone++;
            }

            if (_amountOfTurnsDone >= _stateData.amountOfTurns)
                _isAllTurnsDone = true;

            if (Time.time >= _lastTurnTime + _stateData.timeBetweenTurns && _isAllTurnsDone)
                IsAllTurnsTimeDone = true;
        }
        
        public void SetTurnImmediately(bool flip) => _turnImmediately = flip;
    }
}