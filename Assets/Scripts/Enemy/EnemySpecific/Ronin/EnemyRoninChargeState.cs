using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SubState;
using Scripts.Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ronin
{
    public class EnemyRoninChargeState : ChargeState
    {
        private readonly EnemyRonin _enemyRonin;

        public EnemyRoninChargeState(Scripts.Enemy.EnemyStateMachine.Enemy enemyBase, 
            Scripts.Enemy.EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_ChargeState stateData, 
            EnemyRonin enemyRonin) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRonin = enemyRonin;

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if(PerformCloseRangeAction)
                StateMachine.ChangeState(_enemyRonin.MeleeAttackState);
            else if(!IsDetectingLedge || IsDetectingWall)
                StateMachine.ChangeState(_enemyRonin.LookForPlayerState);
            else if (IsChargeTimeOver)
            {
                if(IsPlayerInAgroRange)
                    StateMachine.ChangeState(_enemyRonin.PlayerDetectedState);
                else StateMachine.ChangeState(_enemyRonin.LookForPlayerState);
            }
        }
    }
}