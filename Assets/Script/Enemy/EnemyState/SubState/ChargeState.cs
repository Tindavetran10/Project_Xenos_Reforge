using Script.Enemy.EnemyState.State_Data;
using Script.Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Script.Enemy.EnemyState.SubState
{
    public class ChargeState : BattleState
    {
        protected bool IsChargeTimeOver;
        
        private readonly D_ChargeState _stateData;
        
        protected ChargeState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_ChargeState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        public override void Enter()
        {
            base.Enter();

            IsChargeTimeOver = false;
            
            Movement?.SetVelocityX(_stateData.chargeSpeed * Movement.FacingDirection);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            Movement?.SetVelocityX(_stateData.chargeSpeed * Movement.FacingDirection);

            if (Time.time >= StartTime + _stateData.chargeTime)
                IsChargeTimeOver = true;
        }
    }
}