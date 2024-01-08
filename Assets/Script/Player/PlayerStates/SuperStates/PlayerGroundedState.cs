using Script.Player.Data;
using Script.Player.PlayerStateMachine;

namespace Script.Player.PlayerStates.SuperStates
{
    // This state will represent all States that happen when the player standing on the ground
    // All of its function gonna be inherited from all of those States 
    public class PlayerGroundedState : PlayerState
    {
        protected int XInput;
        protected int YInput;
        protected bool IsTouchingCeiling;
        
        private bool _jumpInput;
        private bool _grabInput;
        private bool _isGrounded;
        private bool _isTouchingWall;
        private bool _isTouchingLedge;
        private bool _dashInput;

        private bool _normalAttackInput;
        private bool _counterAttackInput;
        
        protected PlayerGroundedState(PlayerStateMachine.Player player, PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName){}

        // Receive the function the DoChecks() from PlayerState
        // and adding the specific needs for the Grounded State
        protected override void DoChecks()
        {
            base.DoChecks();
            if (CollisionSenses)
            {
                _isGrounded = CollisionSenses.Ground;
                _isTouchingWall = CollisionSenses.WallFront;
                _isTouchingLedge = CollisionSenses.LedgeHorizontal;
                IsTouchingCeiling = CollisionSenses.Ceiling;
            }
        }

        // Before entering the specific state that happen on the ground
        // Player will enter this state first
        public override void Enter()
        {
            base.Enter();
            // When the player standing on the ground,
            // reset the number of jumps and dashes that he can make
            Player.JumpState.ResetAmountOfJumpsLeft();
            Player.DashState.ResetCanDash();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Get the input values from the PlayerInputHandler class
            XInput = Player.InputHandler.NormInputX;
            YInput = Player.InputHandler.NormInputY;
            _jumpInput = Player.InputHandler.JumpInput;
            _grabInput = Player.InputHandler.GrabInput;
            _dashInput = Player.InputHandler.DashInput;
            
            _normalAttackInput = Player.InputHandler.AttackInputs[(int)CombatInputs.Normal];

            _counterAttackInput = Player.InputHandler.CounterInput;
            
            
            if(_counterAttackInput && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.CounterAttackState);
            else if(_normalAttackInput && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.PrimaryAttackState);
            
            // Change to Jump State if there is a jumpInput, the number of jumps is > 0 and he isn't touch the ceiling 
            else if(_jumpInput && Player.JumpState.CanJump() && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.JumpState);
            // Change to InAirState if the player isn't standing on the ground
            else if (!_isGrounded)
            {
                // Start the coyote time for receiving jumpInput
                Player.InAirState.StartCoyoteTimer();
                StateMachine.ChangeState(Player.InAirState);
            }
            // Change to Wall Grab State if detecting a wall and receiving grab input
            else if (_isTouchingWall && _grabInput && _isTouchingLedge)
                StateMachine.ChangeState(Player.WallGrabState);
            // Change to Dash State if there is dashInput and the dash has been cooled down
            else if (_dashInput && Player.DashState.CheckIfCanDash() && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.DashState);
        }
    }
}
