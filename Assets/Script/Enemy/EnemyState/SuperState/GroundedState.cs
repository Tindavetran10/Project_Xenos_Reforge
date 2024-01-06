using Script.CoreSystem.CoreComponents;
using UnityEngine;

namespace Script.Enemy.EnemyState.SuperState
{
    public class GroundedState : EnemyStateMachine.EnemyState
    {
        protected bool IsPlayerInAgroRange;
        
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
            : Core.GetCoreComponent(ref _collisionSenses);

        private Movement _movement;
        private CollisionSenses _collisionSenses;
        
        
        protected GroundedState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
        {
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInAgroRange = EnemyBase.CheckPlayerInAgroRange();
        }

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

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}