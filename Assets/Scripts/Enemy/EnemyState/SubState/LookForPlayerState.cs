using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Enemy.EnemyState.SubState
{
    public class LookForPlayerState : GroundedState
    {
        private bool _turnImmediately;
        private bool _isAllTurnsDone;
        protected bool IsAllTurnsTimeDone;

        private float _lastTurnTime;
        private int _amountOfTurnsDone;
        
        private readonly D_LookForPlayerState _stateData;
        
        protected LookForPlayerState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_LookForPlayerState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;
        
        public override void Enter()
        {
            base.Enter();
            EnemyBase.CloseCounterAttackWindow();
            
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
                if (EnemyBase.OnFlipped != null)
                    EnemyBase.OnFlipped();
                Movement?.Flip();
                _lastTurnTime = Time.time;
                _amountOfTurnsDone++;
                _turnImmediately = false;
            } 
            else if (Time.time >= _lastTurnTime + _stateData.timeBetweenTurns && !_isAllTurnsDone)
            {
                if (EnemyBase.OnFlipped != null)
                    EnemyBase.OnFlipped();
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