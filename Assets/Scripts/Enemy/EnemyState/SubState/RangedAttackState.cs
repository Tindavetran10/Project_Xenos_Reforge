using Enemy.EnemyState.StateData;
using Enemy.EnemyState.SuperState;

namespace Enemy.EnemyState.SubState
{
    public class RangedAttackState : BattleState
    {
        private D_RangedAttackState _stateData;
        
        protected RangedAttackState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_RangedAttackState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;
    }
}