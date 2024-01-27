using System;
using System.Linq;
using Player.Data;
using Player.PlayerStates.SuperStates;
using Scripts.Enemy.EnemyStats;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerPrimaryAttackState : PlayerAbilityState
    {
        #region Combo variables
        private bool _attackInputStop;
        private float _lastTimeAttacked;
        private static readonly int Counter = Animator.StringToHash("comboCounter");
        #endregion
        
        public PlayerPrimaryAttackState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}
        
        public override void Enter()
        {
            base.Enter();
            IsHolding = false;
            StartTime = Time.time;
            Player.InputHandler.UseAttackInput();
            
            if (ComboCounter >= PlayerData.numberOfAttacks || Time.time >= _lastTimeAttacked + PlayerData.comboWindow)
                ComboCounter = 0;
            
            Player.Anim.SetInteger(Counter, ComboCounter);
        }

        public override void Exit()
        {
            base.Exit();

            ComboCounter++;
            _lastTimeAttacked = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (_attackInputStop || Time.time >= StartTime + PlayerData.maxHoldTime)
                IsHolding = false;

            if (IsAnimationFinished || IsAnimationCancel)
                IsAbilityDone = true;
        }
        
        #region Animation Functions
        public override void AnimationCancelTrigger()
        {
            base.AnimationCancelTrigger();

            foreach (var combatInput in Enum.GetValues(typeof(CombatInputs)).Cast<CombatInputs>())
            {
                if (Player.InputHandler.AttackInputs[(int)combatInput] ||
                    Player.InputHandler.NormInputX == 1 || Player.InputHandler.NormInputX == -1 ||
                    Player.InputHandler.JumpInput ||
                    Player.InputHandler.DashInput)
                {
                    IsAnimationCancel = true;
                    break; // Exit the loop if any condition is met
                }
            }
        }
        
        public override void StartMovementTrigger()
        {
            base.StartMovementTrigger();
            Movement.SetVelocity(PlayerData.attackVelocity[ComboCounter], PlayerData.direction[ComboCounter], Movement.FacingDirection);
        }

        public override void StopMovementTrigger()
        {
            base.StopMovementTrigger();
            Movement.SetVelocityZero();
        }

        public override void SetFlipActive()
        {
            base.SetFlipActive();
            Movement.CheckIfShouldFlip(Player.InputHandler.NormInputX);
        }

        public override void SetFlipInactive()
        {
            base.SetFlipInactive();
            Movement.CheckIfShouldFlip(0);
        }
        #endregion
        
        public override void AttackTrigger()
        {
            base.AttackTrigger();

            var playerTransform = Player.attackPosition.transform;
            var playerPosition = playerTransform.position;

            Offset.Set(playerPosition.x + PlayerData.hitBox[ComboCounter].center.x * Movement.FacingDirection,
                playerPosition.y + PlayerData.hitBox[ComboCounter].center.y);
            
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Offset, PlayerData.hitBox[ComboCounter].size, 0f, PlayerData.whatIsEnemy);

            foreach (var hit in collider2Ds)
            {
                if (hit.GetComponent<Scripts.Enemy.EnemyStateMachine.Enemy>() != null)
                {
                    var target = hit.GetComponentInChildren<EnemyStats>();
                    Player.Stats.DoDamage(target);
                }
            }
        }
    }
}