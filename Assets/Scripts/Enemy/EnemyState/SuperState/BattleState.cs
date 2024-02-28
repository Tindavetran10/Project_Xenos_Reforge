using UnityEngine;

namespace Enemy.EnemyState.SuperState
{
    public class BattleState : global::Enemy.EnemyStateMachine.EnemyState
    {
        protected bool IsPlayerInAgroRange;
        protected bool IsDetectingLedge;
        protected bool IsDetectingWall;
        protected bool IsGrounded;
        
        protected bool PerformLongRangeAction;
        protected bool PerformCloseRangeAction;

        protected int _comboCounter;
        protected int _comboWindow;
        protected static readonly int ComboCounter = Animator.StringToHash("comboCounter");
        protected BattleState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName) : base(enemyBase, stateMachine, animBoolName) {}

        protected override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInAgroRange = EnemyBase.CheckPlayerInAgroRange();
            IsDetectingLedge = CollisionSenses.LedgeVertical;
            IsDetectingWall = CollisionSenses.WallFront;
            IsGrounded = CollisionSenses.Ground;
            
            PerformCloseRangeAction = EnemyBase.CheckPlayerInCloseRangeAction();
        }
        
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityX(0f);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            EnemyBase.BattleStateFlipControl();
        }
    }
}