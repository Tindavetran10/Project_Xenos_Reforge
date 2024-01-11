using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SuperState;

namespace Script.Enemy.EnemyState.SubState
{
    public class RangedAttackState : BattleState
    {
        private D_RangedAttackState _stateData;
        
        protected RangedAttackState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_RangedAttackState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;
        
        
    }
}