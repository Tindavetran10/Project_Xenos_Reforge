using _Scripts.Enemies.EnemyState.StateData;
using Script.Enemy.EnemyState.SubState;

namespace Script.Enemy.EnemySpecific.Ronin
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
            if(IsPlayerInAgroRange)
                StateMachine.ChangeState(_enemyRonin.PlayerDetectedState);
            else if (!IsDetectingLedge || IsDetectingWall || IsMoveTimeOver)
            {
                _enemyRonin.IdleState.SetFlipAfterIdle(true);
                StateMachine.ChangeState(_enemyRonin.IdleState);
            }
        }
    }
}