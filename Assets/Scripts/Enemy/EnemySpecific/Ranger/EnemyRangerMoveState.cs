using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerMoveState : MoveState
    {
        private readonly EnemyRanger _enemyRanger;

        public EnemyRangerMoveState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_MoveState stateData, 
            EnemyRanger enemyRanger) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRanger = enemyRanger;

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsPlayerInAgroRange)
                StateMachine.ChangeState(_enemyRanger.PlayerDetectedState);
            else if (!IsDetectingLedge || IsDetectingWall || IsMoveTimeOver)
            {
                _enemyRanger.IdleState.SetFlipAfterIdle(true);
                StateMachine.ChangeState(_enemyRanger.IdleState);
            }
        }
    }
}