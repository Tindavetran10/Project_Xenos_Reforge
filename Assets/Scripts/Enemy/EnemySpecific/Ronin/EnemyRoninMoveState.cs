using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ronin
{
    public class EnemyRoninMoveState : MoveState
    {
        private readonly EnemyRonin _enemyRonin;

        public EnemyRoninMoveState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_MoveState enemyData, 
            EnemyRonin enemyRonin) : base(enemyBase, stateMachine, animBoolName, enemyData) =>
            _enemyRonin = enemyRonin;

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(PerformLongRangeAction)
                StateMachine.ChangeState(_enemyRonin.PlayerDetectedState);
            else if (!IsDetectingLedge || IsDetectingWall || IsMoveTimeOver)
            {
                _enemyRonin.IdleState.SetFlipAfterIdle(true);
                StateMachine.ChangeState(_enemyRonin.IdleState);
            }
        }
    }
}