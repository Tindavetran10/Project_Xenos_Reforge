using CoreSystem.CoreComponents;

namespace Enemy.EnemyState.SuperState
{
    public class GroundedState : global::Enemy.EnemyStateMachine.EnemyState
    {
        protected bool IsDetectingLedge;
        protected bool IsDetectingWall;
        
        protected bool PerformLongRangeAction;

        private Movement _movement;
        private CollisionSenses _collisionSenses;
        
        protected GroundedState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName) : base(enemyBase, stateMachine, animBoolName) {}

        protected override void DoChecks()
        {
            base.DoChecks();
            IsDetectingLedge = CollisionSenses.LedgeVertical;
            IsDetectingWall = CollisionSenses.WallFront;
            
            EnemyBase.CheckPlayerInCloseRangeAction();
            PerformLongRangeAction = EnemyBase.CheckPlayerInAgroRange();
        }
    }
}