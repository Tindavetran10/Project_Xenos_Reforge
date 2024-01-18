using Script.Player.Data;
using Script.Player.PlayerStateMachine;
using UnityEngine;

namespace Script.Player.PlayerStates.SubStates
{
    public class PlayerLedgeClimbState: PlayerState
    {
        private Vector2 _detectedPos;
        private Vector2 _cornerPos;
        private Vector2 _startPos;
        private Vector2 _stopPos;
        private Vector2 _workspace;
        
        private bool _isHanging;
        private bool _isClimbing;
        private bool _jumpInput;
        private bool _isTouchingCeiling;

        private int _xInput;
        private int _yInput;
        
        private static readonly int ClimbLedge = Animator.StringToHash("climbLedge");
        private static readonly int IsTouchingCeiling = Animator.StringToHash("isTouchingCeiling");

        public PlayerLedgeClimbState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}


        public void SetDetectedPosition(Vector2 pos) => _detectedPos = pos; 
        
        public override void Enter() {
            base.Enter();

            Movement?.SetVelocityZero();
            Player.transform.position = _detectedPos;
            _cornerPos = DetermineCornerPosition();

            // Offset the image of Player to be next to the ledge

            if (Movement != null)
            {
                _startPos.Set(_cornerPos.x - Movement.FacingDirection * PlayerData.startOffset.x,
                    _cornerPos.y - PlayerData.startOffset.y);
                _stopPos.Set(_cornerPos.x + Movement.FacingDirection * PlayerData.stopOffset.x,
                    _cornerPos.y + PlayerData.stopOffset.y);
            }

            Player.transform.position = _startPos;
        }

        public override void Exit() {
            base.Exit();

            _isHanging = false;

            if (_isClimbing) {
                Player.transform.position = _stopPos;
                _isClimbing = false;
            }
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (IsAnimationFinished) 
            {
                if (_isTouchingCeiling)
                    StateMachine.ChangeState(Player.CrouchIdleState);
                else StateMachine.ChangeState(Player.IdleState);
            } 
            else 
            {
                _xInput = Player.InputHandler.NormInputX;
                _yInput = Player.InputHandler.NormInputY;
                _jumpInput = Player.InputHandler.JumpInput;

                Movement?.SetVelocityZero();
                Player.transform.position = _startPos;

                if (Movement != null && _xInput == Movement.FacingDirection && _isHanging && !_isClimbing) 
                {
                    CheckForSpace();
                    _isClimbing = true;
                    Player.Anim.SetBool(ClimbLedge, true);
                } 
                else if (_yInput == -1 && _isHanging && !_isClimbing)
                    StateMachine.ChangeState(Player.InAirState);
                else if (Movement != null && _xInput == -Movement.FacingDirection && _jumpInput && !_isClimbing) 
                {
                    Player.WallJumpState.DetermineWallJumpDirection(true);
                    StateMachine.ChangeState(Player.WallJumpState);
                }
            }
        }

        public override void AnimationTrigger() {
            base.AnimationTrigger();
            _isHanging = true;
        }

        public override void AnimationFinishTrigger() {
            base.AnimationFinishTrigger();
            Player.Anim.SetBool(ClimbLedge, false);
        }
        
        private void CheckForSpace() {
            _isTouchingCeiling = Physics2D.Raycast(
                _cornerPos + Vector2.up * 0.02f + 
                Vector2.right * (Movement.FacingDirection * 0.02f), 
                Vector2.up, PlayerData.standColliderHeight, 
                CollisionSenses.WhatIsGround);
            Player.Anim.SetBool(IsTouchingCeiling, _isTouchingCeiling);
        }
        
        private Vector2 DetermineCornerPosition() {
            // Shoot a raycast to detect a wall
            var wallCheckPosition = CollisionSenses.WallCheck.position;
            var xHit = Physics2D.Raycast(
                wallCheckPosition, 
                Vector2.right * Movement.FacingDirection, 
                CollisionSenses.WallCheckDistance, 
                CollisionSenses.WhatIsGround);
            
            // Save the length of raycast from player to wall
            var xDist = xHit.distance;
            
            // Save the value to work space base on the facing direction
            _workspace.Set((xDist + 0.015f) * Movement.FacingDirection, 0f);
            
            // Shoot an another raycast with a specific length downward from the ending point of the X raycast's distance
            var ledgeCheckHorizontalPosition = CollisionSenses.LedgeCheckHorizontal.position;
            var yHit = Physics2D.Raycast(
                ledgeCheckHorizontalPosition + 
                (Vector3)_workspace, 
                Vector2.down, 
                ledgeCheckHorizontalPosition.y - wallCheckPosition.y + 0.02f, 
                CollisionSenses.WhatIsGround);

            // Save the length of raycast
            var yDist = yHit.distance;

            _workspace.Set(
                wallCheckPosition.x + 
                xDist * Movement.FacingDirection, 
                ledgeCheckHorizontalPosition.y - yDist);
            
            return _workspace;
        }
    }
}