using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Enemy.EnemyState.SubState
{
    public class GetAttackedState : BattleState
    {
        protected bool IsGetAttackedTimeOver;
        
        private readonly D_GetAttacked _stateData;
        
        protected GetAttackedState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine,
            string animBoolName, D_GetAttacked stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        
        public override void Enter()
        {
            base.Enter();
            IsGetAttackedTimeOver = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (Time.time >= StartTime + _stateData.getAttackedTime) 
                IsGetAttackedTimeOver = true;
            
        }
    }
}