using Player.Data;
using Player.PlayerStateMachine;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerInAirState : PlayerState
    {
        #region Input
        private int _xInput;
        private bool _jumpInput;
        private bool _jumpInputStop;
        private bool _dashInput;
        #endregion
    
        #region Checks
        private bool _isGrounded;
        private bool _isJumping;
        private bool _isTouchingWall;
        private bool _isTouchingWallBack;
        private bool _oldIsTouchingWall;
        private bool _oldIsTouchingWallBack;
        private bool _isTouchingLedge;

        private bool _coyoteTimer;
        private bool _wallJumpCoyoteTimer;

        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        #endregion
    
        public PlayerInAirState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}

        protected override void DoChecks() {
            base.DoChecks();

            _oldIsTouchingWall = _isTouchingWall;
            _oldIsTouchingWallBack = _isTouchingWallBack;

            if (CollisionSenses)
            {
                _isGrounded = CollisionSenses.Ground;
                _isTouchingWall = CollisionSenses.WallFront;
                _isTouchingWallBack = CollisionSenses.WallBack;
                _isTouchingLedge = CollisionSenses.LedgeHorizontal;
            }
            
            // Save the X and Y axis of Player when we detect a ledge 
            if(_isTouchingWall && !_isTouchingLedge)
                Player.LedgeClimbState.SetDetectedPosition(Player.transform.position);
            
            if (!_wallJumpCoyoteTimer && !_isTouchingWall && !_isTouchingWallBack &&
                (_oldIsTouchingWall || _oldIsTouchingWallBack)) 
                StartWallJumpCoyoteTimer();
            
        }

        public override void Exit()
        {
            base.Exit();
            _oldIsTouchingWall = false;
            _oldIsTouchingWallBack = false;
            _isTouchingWall = false;
            _isTouchingWallBack = false;
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
        
            CheckCoyoteTime();
            CheckWallJumpCoyoteTime();
        
            // Get the input values from the PlayerInputHandler class
            _xInput = Player.inputManager.NormInputX;
            _jumpInput = Player.inputManager.JumpInput;
            _jumpInputStop = Player.inputManager.JumpInputStop;
            _dashInput = Player.inputManager.DashInput;
        
            CheckJumpMultiplier();
            
            if (_isGrounded && Movement?.CurrentVelocity.y < 0.01f)
                StateMachine.ChangeState(Player.LandState);
            
            // Change to Ledge Climb State if he the Ledge of Wall 
            else if (_isTouchingWall && !_isTouchingLedge)
                StateMachine.ChangeState(Player.LedgeClimbState);
            
            // Change to Wall Jump State depends if
            // there is jumpInput
            // he touching either the wall on the back or in the front
            // the WallCoyoteTime is still running
            else if (_jumpInput && (_isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTimer))
            {
                // Stop Wall Jump Coyote Time, stop receiving WallJump input
                StopWallJumpCoyoteTimer();
                // Set the the direction for the next wall jump
                _isTouchingWall = CollisionSenses.WallFront;
                Player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
                
                StateMachine.ChangeState(Player.WallJumpState);
            }
            
            // Change to Jump State if there is a jumpInput, the number of jumps is > 0
            // This is for when the player doing a double jump
            else if (_jumpInput && Player.JumpState.CanJump())
                StateMachine.ChangeState(Player.JumpState);
            
            // Change to WallSlide state if he touching a wall
            // there is x input moving toward the wall and receive no y input for jumping
            else if (_isTouchingWall && _xInput == Movement?.FacingDirection && Movement?.CurrentVelocity.y <= 0)
                StateMachine.ChangeState(Player.WallSlideState);
            
            // Change to Dash State if the dash has been cooled down
            else if (_dashInput && Player.DashState.CheckIfCanDash())
                StateMachine.ChangeState(Player.DashState);
            else
            {
                // Change the velocity while the player in the air
                Movement?.CheckIfShouldFlip(_xInput);
                Movement?.SetVelocityX(PlayerData.movementVelocity * _xInput);
                
                // Play the animation base on the velocity
                if (Movement == null) return;
                Player.Anim.SetFloat(YVelocity, Movement.CurrentVelocity.y);
                Player.Anim.SetFloat(XVelocity, Mathf.Abs(Movement.CurrentVelocity.x));
            }  
        }

        // Cut the jump if the player stop holding the jump button
        private void CheckJumpMultiplier()
        {
            // If the player is jumping
            if (_isJumping)
            {
                // the jump input suddenly stop
                if (_jumpInputStop)
                {
                    // Reduce the velocity of y immediately
                    Movement?.SetVelocityY(Movement.CurrentVelocity.y * PlayerData.variableJumpHeightMultiplier);
                    _isJumping = false;
                }
                else if (Movement.CurrentVelocity.y <= 0f)
                    _isJumping = false;
            }
        }

        private void CheckCoyoteTime()
        {
            // If the coyoteTimer is running (true)
            // And Time.time > (time that the player enter the InAirState + value of CoyoteTime)
            // which mean the coyote time is over
            if (_coyoteTimer && Time.time > StartTime + PlayerData.coyoteTime)
            {
                // Stop the coyote timer and decrease the number of jump
                StopCoyoteTimer();
                Player.JumpState.DecreaseAmountOfJumpsLeft();
            }
        }
        
        private void CheckWallJumpCoyoteTime()
        {
            // If the coyoteTimer is running (true)
            // And Time.time > (time that the player enter the InAirState + value of CoyoteTime)
            // which mean the coyote time is over
            if (_wallJumpCoyoteTimer && Time.time > StartTime + PlayerData.coyoteTime)
                StopWallJumpCoyoteTimer();
        }

        public void StartCoyoteTimer() => _coyoteTimer = true;
        private void StartWallJumpCoyoteTimer() => _wallJumpCoyoteTimer = true;

        private void StopCoyoteTimer() => _coyoteTimer = false;
        private void StopWallJumpCoyoteTimer() => _wallJumpCoyoteTimer = false;
        public void SetIsJumping() => _isJumping = true;
    }
}
