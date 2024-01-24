using UnityEngine;

namespace Scripts.Enemy.EnemyState.SuperState
{
    public class BattleState : EnemyStateMachine.EnemyState
    {
        //protected bool IsAnimationFinished;
        protected bool IsPlayerInAgroRange;
        protected bool IsDetectingLedge;
        protected bool IsDetectingWall;
        protected bool IsGrounded;
        
        protected bool PerformLongRangeAction;
        protected bool PerformCloseRangeAction;

        private float _lastTimeAttacked;
        
        private int _comboCounter;
        protected int ComboWindow;
        private static readonly int ComboCounter = Animator.StringToHash("comboCounter");
        
        protected BattleState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
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
            if (_comboCounter > ComboWindow || Time.time >= _lastTimeAttacked + ComboWindow)
            {
                _lastTimeAttacked = Time.time;
                _comboCounter = 0;
            }

            EnemyBase.Anim.SetInteger(ComboCounter, _comboCounter);
            EnemyBase.isAnimationFinished = false;
            
            Movement?.SetVelocityX(0f);
        }

        public override void Exit()
        {
            base.Exit();
            _comboCounter++;
        }
    }
}