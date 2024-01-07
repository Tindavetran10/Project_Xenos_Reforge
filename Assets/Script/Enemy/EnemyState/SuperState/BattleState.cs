using Script.CoreSystem.CoreComponents;
using Script.Enemy.Data;
using UnityEngine;

namespace Script.Enemy.EnemyState.SuperState
{
    public class BattleState : EnemyStateMachine.EnemyState
    {
        protected bool IsAnimationFinished;
        protected bool IsPlayerInAgroRange;
        protected bool IsDetectingLedge;
        protected bool IsDetectingWall;
        
        protected bool PerformLongRangeAction;
        protected bool PerformCloseRangeAction;

        private float _lastTimeAttacked;

        private int _comboCounter;
        protected int ComboWindow;
        private static readonly int ComboCounter = Animator.StringToHash("comboCounter");

        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;

        private CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
            : Core.GetCoreComponent(ref _collisionSenses);
        private CollisionSenses _collisionSenses;
        
        protected BattleState(EnemyStateMachine.Enemy enemyBase, EnemyStateMachine.EnemyStateMachine stateMachine, 
            string animBoolName) : base(enemyBase, stateMachine, animBoolName) {}

        protected override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInAgroRange = EnemyBase.CheckPlayerInAgroRange();
            IsDetectingLedge = CollisionSenses.LedgeVertical;
            IsDetectingWall = CollisionSenses.WallFront;
            
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
            EnemyBase.Atsm.BattleState = this;
            IsAnimationFinished = false;
            
            Movement?.SetVelocityX(0f);
        }

        public override void Exit()
        {
            base.Exit();
            _comboCounter++;
        }

        public void FinishAttack() => IsAnimationFinished = true;

        public void AttackTrigger()
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(EnemyBase.attackPosition.position, EnemyBase.enemyData.hitBox.Length);

            foreach (var hit in collider2Ds)
            {
                if(hit.GetComponent<Player.PlayerStateMachine.Player>() != null)
                    hit.GetComponent<Player.PlayerStateMachine.Player>().Damage();
            }
        }
    }
}