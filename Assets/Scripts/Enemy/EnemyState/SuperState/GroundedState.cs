using CoreSystem.CoreComponents;

namespace Enemy.EnemyState.SuperState
{
    public class GroundedState : global::Enemy.EnemyStateMachine.EnemyState
    {
        protected bool IsPlayerInAgroRange;
        protected bool IsDetectingLedge;
        protected bool IsDetectingWall;
        
        private Movement _movement;
        private CollisionSenses _collisionSenses;
        
        protected GroundedState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName) : base(enemyBase, stateMachine, animBoolName) {}

        protected override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInAgroRange = EnemyBase.CheckPlayerInAgroRange();
            IsDetectingLedge = CollisionSenses.LedgeVertical;
            IsDetectingWall = CollisionSenses.WallFront;
        }
    }
}