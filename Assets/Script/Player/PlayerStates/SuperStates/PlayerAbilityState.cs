﻿using Script.Player.Data;
using Script.Player.PlayerStateMachine;
using UnityEngine;

namespace Script.Player.PlayerStates.SuperStates
{
    public class PlayerAbilityState : PlayerState {
        
        protected Vector2 Offset;
        protected int ComboCounter;
        
        private bool _isGrounded;
        protected bool IsAbilityDone;
        
        protected bool IsHolding;
        
        protected PlayerAbilityState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName){}

        protected override void DoChecks() {
            base.DoChecks();

            if (CollisionSenses) 
                _isGrounded = CollisionSenses.Ground;
        }

        public override void Enter() {
            if(Player.isActiveAndEnabled == false)
                return;
            base.Enter();
            IsAbilityDone = false;
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            
            if (IsAbilityDone)
            {
                if (_isGrounded && Movement != null && Movement.CurrentVelocity.y < 0.01f)
                    StateMachine.ChangeState(Player.IdleState);
                else StateMachine.ChangeState(Player.InAirState);
            }
        }
    }
}
