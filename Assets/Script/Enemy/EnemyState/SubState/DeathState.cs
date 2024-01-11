using Script.Enemy.EnemyState.SuperState;

namespace Script.Enemy.EnemyState.SubState
{
    public class DeathState : GroundedState
    {
        protected DeathState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName) : base(enemyBase, stateMachine, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
        }
    }
}