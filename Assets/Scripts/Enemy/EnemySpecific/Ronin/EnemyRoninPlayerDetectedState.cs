using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ronin
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
            switch (PerformLongRangeAction)
            {
                case false:
                    StateMachine.ChangeState(_enemyRonin.LookForPlayerState);
                    break;
                case true when !IsDetectingWall:
                    StateMachine.ChangeState(_enemyRonin.ChargeState);
                    break;
                default:
                {
                    if (PerformCloseRangeAction)
                        StateMachine.ChangeState(_enemyRonin.MeleeAttackState);
                    else if (!IsDetectingLedge)
                    {
                        Movement?.Flip();
                        StateMachine.ChangeState(_enemyRonin.MoveState);
                    }
                    break;
                }
            }
        }
    }
}