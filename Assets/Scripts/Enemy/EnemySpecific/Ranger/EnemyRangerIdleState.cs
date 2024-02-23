using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SubState;

namespace Enemy.EnemySpecific.Ranger
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
            else if (IsIdleTimeOver) 
                StateMachine.ChangeState(_enemyRanger.MoveState);
        }
    }
}