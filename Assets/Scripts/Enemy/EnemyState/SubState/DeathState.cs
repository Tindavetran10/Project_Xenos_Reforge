using Scripts.Enemy.EnemyState.SuperState;

namespace Enemy.EnemyState.SubState
{
    public class DeathState : GroundedState
    {
        protected DeathState(Scripts.Enemy.EnemyStateMachine.Enemy enemyBase, Scripts.Enemy.EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName) : base(enemyBase, stateMachine, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
        }
    }
}