using System;
using System.Linq;
using Script.Player.Data;
using Script.Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Script.Player.PlayerStates.SubStates
{
    public class PlayerPrimaryAttackState : PlayerAbilityState
    {
        #region Combo variables
        private int _comboCounter;

        private float _lastTimeAttacked;
        private static readonly int ComboCounter = Animator.StringToHash("comboCounter");
        #endregion

        private Vector2 _offset;
        
        public PlayerPrimaryAttackState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}
        
        public override void Enter()
        {
            base.Enter();

            if (_comboCounter >= PlayerData.numberOfAttacks || Time.time >= _lastTimeAttacked + PlayerData.comboWindow)
                _comboCounter = 0;
            
            Debug.Log(_comboCounter);
            
            Player.Anim.SetInteger(ComboCounter, _comboCounter);
        }

        public override void Exit()
        {
            base.Exit();

            _comboCounter++;
            _lastTimeAttacked = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsAnimationFinished || IsAnimationCancel)
                IsAbilityDone = true;

        }
        
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
            Movement.SetVelocity(PlayerData.attackVelocity[_comboCounter], PlayerData.direction[_comboCounter], Movement.FacingDirection);
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

        public override void AttackTrigger()
        {
            base.AttackTrigger();

            var playerTransform = Player.attackPosition.transform;
            var playerPosition = playerTransform.position;

            _offset.Set(playerPosition.x + PlayerData.hitBox[_comboCounter].center.x * Movement.FacingDirection,
                playerPosition.y + PlayerData.hitBox[_comboCounter].center.y);
            
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(_offset, PlayerData.hitBox[_comboCounter].size, 0f, PlayerData.whatIsEnemy);

            foreach (var hit in collider2Ds)
            {
                if (hit.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null)
                    hit.GetComponent<Enemy.EnemyStateMachine.Enemy>().Damage();
            }
        }
    }
}