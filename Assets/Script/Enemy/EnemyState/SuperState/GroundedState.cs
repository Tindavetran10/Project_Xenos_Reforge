using Script.CoreSystem.CoreComponents;

namespace Script.Enemy.EnemyState.SuperState
{
    public class GroundedState : EnemyStateMachine.EnemyState
    {
        protected bool IsPlayerInAgroRange;
        protected bool IsDetectingLedge;
        protected bool IsDetectingWall;

        
        
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
            : Core.GetCoreComponent(ref _collisionSenses);

        private Movement _movement;
        private CollisionSenses _collisionSenses;
        
        
        protected GroundedState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
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