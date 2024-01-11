using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SuperState;

namespace Script.Enemy.EnemyState.SubState
{
    public class RangerAttackState : BattleState
    {
        private D_RangedAttackState _stateData;
        
        protected RangerAttackState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
        {
        }
    }
}