using Enemy.EnemyState.StateData;
using Scripts.Enemy.EnemyState.SuperState;
using UnityEngine;

namespace Enemy.EnemyState.SubState
{
    public class ChargeState : BattleState
    {
        protected bool IsChargeTimeOver;
        
        private readonly D_ChargeState _stateData;
        
        protected ChargeState(EnemyStateMachine.Enemy enemyBase, Scripts.Enemy.EnemyStateMachine.EnemyStateMachine stateMachine, 
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