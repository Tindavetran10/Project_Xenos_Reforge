using UnityEngine;

namespace Enemy.EnemyState.SuperState
{
    public class BattleState : global::Enemy.EnemyStateMachine.EnemyState
    {
        protected bool IsDetectingLedge;
        protected bool IsDetectingWall;
        protected bool IsGrounded;
        
        protected bool PerformLongRangeAction;
        protected bool PerformCloseRangeAction;

        protected int _comboCounter;
        protected int _comboWindow;
        
        protected bool IsAttacked;
        protected bool IsGetAttackedTimeOver;
        
        protected static readonly int ComboCounter = Animator.StringToHash("comboCounter");
        protected BattleState(global::Enemy.EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName) : base(enemyBase, stateMachine, animBoolName) {}

        protected override void DoChecks()
        {
            base.DoChecks();
            IsDetectingLedge = CollisionSenses.LedgeVertical;
            IsDetectingWall = CollisionSenses.WallFront;
            IsGrounded = CollisionSenses.Ground;

            IsAttacked = EnemyBase.isAttacked;
            IsGetAttackedTimeOver = EnemyBase.isGetAttackedTimeOver;
            
            PerformCloseRangeAction = EnemyBase.CheckPlayerInCloseRangeAction();
            PerformLongRangeAction = EnemyBase.CheckPlayerInAgroRange();
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