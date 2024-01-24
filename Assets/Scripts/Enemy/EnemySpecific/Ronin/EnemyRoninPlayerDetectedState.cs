using Enemy.EnemyState.StateData;
using Scripts.Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ronin
{
    public class EnemyRoninPlayerDetectedState : PlayerDetectedState
    {
        private readonly EnemyRonin _enemyRonin;

        public EnemyRoninPlayerDetectedState(Scripts.Enemy.EnemyStateMachine.Enemy enemyBase, 
            Scripts.Enemy.EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, 
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