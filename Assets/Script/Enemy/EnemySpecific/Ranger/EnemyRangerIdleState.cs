using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SubState;

namespace Script.Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerIdleState : IdleState
    {
        private readonly EnemyRanger _enemyRanger;

        public EnemyRangerIdleState(EnemyStateMachine.Enemy enemyBase, 
            EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_IdleState stateData, 
            EnemyRanger enemyRanger) : base(enemyBase, stateMachine, animBoolName, stateData) =>
            _enemyRanger = enemyRanger;
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(IsPlayerInAgroRange)
                StateMachine.ChangeState(_enemyRanger.PlayerDetectedState);
            else if(IsIdleTimeOver)
                StateMachine.ChangeState(_enemyRanger.MoveState);
        }
    }
}