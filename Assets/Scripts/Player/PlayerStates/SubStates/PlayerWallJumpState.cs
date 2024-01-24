using System;
using Player.Data;
using Player.PlayerStates.SuperStates;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerWallJumpState : PlayerAbilityState
    {
        private int _wallJumpDirection;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        public PlayerWallJumpState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}


        public override void Enter()
        {
            base.Enter();
            Player.InputHandler.UseJumpInput();
            Player.JumpState.ResetAmountOfJumpsLeft();
            Movement?.SetVelocity(PlayerData.wallJumpVelocity, PlayerData.wallJumpAngle, _wallJumpDirection);
            Movement?.CheckIfShouldFlip(_wallJumpDirection);
            Player.JumpState.DecreaseAmountOfJumpsLeft();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            Player.Anim.SetFloat(YVelocity, Movement.CurrentVelocity.y);
            Player.Anim.SetFloat(XVelocity, Math.Abs(Movement.CurrentVelocity.x));

            if (Time.time >= StartTime + PlayerData.wallJumpTime) 
                IsAbilityDone = true;
        }

        public void DetermineWallJumpDirection(bool isTouchingWall)
        {
            if (isTouchingWall)
                _wallJumpDirection = -Movement.FacingDirection;
            else _wallJumpDirection = Movement.FacingDirection;
        }
    }
}
