using Enemy.EnemyState.StateData;
using Scripts.Enemy.EnemyState.SuperState;

namespace Scripts.Enemy.EnemyState.SubState
{
    public class RangedAttackState : BattleState
    {
        private D_RangedAttackState _stateData;
        
        protected RangedAttackState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_RangedAttackState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;
    }
}