﻿using Script.Player.Data;
using Script.Player.PlayerStates.SuperStates;

namespace Script.Player.PlayerStates.SubStates
{
    public class PlayerCrouchMoveState : PlayerGroundedState
    {
        public PlayerCrouchMoveState(Script.Player.PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}

        public override void Enter()
        {
            base.Enter();
            Player.SetColliderHeight(PlayerData.crouchColliderHeight);
        }

        public override void Exit()
        {
            base.Exit();
            Player.SetColliderHeight(PlayerData.standColliderHeight);
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsExitingState)
            {
                Movement?.SetVelocityX(PlayerData.crouchMovementVelocity * Movement.FacingDirection);
                
                Movement?.CheckIfShouldFlip(XInput);
                if (XInput == 0)
                    StateMachine.ChangeState(Player.CrouchIdleState);
                else if (YInput != -1 && !IsTouchingCeiling) 
                    StateMachine.ChangeState(Player.MoveState);
            }
        }
    }
}