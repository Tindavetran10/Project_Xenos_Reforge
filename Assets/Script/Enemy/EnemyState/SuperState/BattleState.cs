namespace Script.Enemy.EnemyState.SuperState
{
    public class BattleState : EnemyStateMachine.EnemyState
    {
        protected BattleState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
        {
        }
    }
}