using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SuperState;

namespace Script.Enemy.EnemyState.SubState
{
    public class MeleeAttackState : BattleState
    {
        private readonly D_MeleeAttackState _stateData;
        
        protected MeleeAttackState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_MeleeAttackState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        public override void Enter()
        {
            base.Enter();
            ComboWindow = _stateData.comboTotal;
        }
    }
}