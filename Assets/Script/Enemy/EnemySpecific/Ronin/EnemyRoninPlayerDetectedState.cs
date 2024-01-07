using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SubState;
using UnityEngine;

namespace Script.Enemy.EnemySpecific.Ronin
{
    public class EnemyRoninPlayerDetectedState : PlayerDetectedState
    {
        private readonly EnemyRonin _enemyRonin;

        public EnemyRoninPlayerDetectedState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, 
            EnemyRonin enemyRonin) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRonin = enemyRonin;

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(PerformCloseRangeAction)
                StateMachine.ChangeState(_enemyRonin.MeleeAttackState);
            else if(PerformLongRangeAction && !IsDetectingWall)
                StateMachine.ChangeState(_enemyRonin.ChargeState);
            else if (!IsPlayerInAgroRange)
                StateMachine.ChangeState(_enemyRonin.LookForPlayerState);
            else if (!IsDetectingLedge)
            {
                Movement?.Flip();
                StateMachine.ChangeState(_enemyRonin.MoveState);
            }
        }

        
    }
}