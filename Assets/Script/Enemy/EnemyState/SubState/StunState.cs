using Script.Enemy.EnemyState.StateData;
using Script.Enemy.EnemyState.SuperState;

namespace Script.Enemy.EnemyState.SubState
{
    public class StunState : BattleState
    {
        private readonly D_StunState _stateData;
        
        protected StunState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_StunState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }
    }
}