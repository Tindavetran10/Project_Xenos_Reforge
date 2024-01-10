using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SubState;

namespace Script.Enemy.EnemySpecific.Ranger
{
    public class EnemyRangerIdleState : IdleState
    {
        protected EnemyRangerIdleState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(enemyBase, stateMachine, animBoolName, stateData)
        {
        }
    }
}