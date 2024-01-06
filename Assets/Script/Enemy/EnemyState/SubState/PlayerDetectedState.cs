using _Scripts.Enemies.EnemyState.StateData;
using Script.CoreSystem.CoreComponents;
using UnityEngine;

namespace Script.Enemy.EnemyState.SubState
{
    public class PlayerDetectedState : EnemyStateMachine.EnemyState
    {
        protected bool IsPlayerInArgoRange;
        protected bool PerformLongRangeAction;
        protected bool PerformCloseRangeAction;
        protected bool IsDetectingLedge;
        
        private readonly D_PlayerDetectedState _stateData;
        
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;


        private CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
            : Core.GetCoreComponent(ref _collisionSenses);

        private CollisionSenses _collisionSenses;


        protected PlayerDetectedState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName, D_PlayerDetectedState stateData) : base(enemyBase, stateMachine, animBoolName) =>
            _stateData = stateData;

        public override void Enter()
        {
            base.Enter();

            PerformLongRangeAction = false;
            Movement?.SetVelocityX(0f);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            Movement?.SetVelocityX(0f);

            if (Time.time >= StartTime + _stateData.longRangeActionTime)
                PerformLongRangeAction = true;
        }

        protected override void DoChecks()
        {
            base.DoChecks();

            IsPlayerInArgoRange = EnemyBase.CheckPlayerInAgroRange();

            IsDetectingLedge = CollisionSenses.LedgeVertical;
            PerformCloseRangeAction = EnemyBase.CheckPlayerInCloseRangeAction();
        }
    }
}