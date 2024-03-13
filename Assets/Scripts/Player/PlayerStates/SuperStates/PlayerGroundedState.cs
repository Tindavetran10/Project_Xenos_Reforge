using Player.Data;
using Player.GhostTrail_Effect;
using Player.PlayerStateMachine;

namespace Player.PlayerStates.SuperStates
{
    // This state will represent all States that happen when the player standing on the ground
    // All of its function gonna be inherited from all of those States 
    public class PlayerGroundedState : PlayerState
    {
        protected int XInput;
        protected int YInput;
        protected bool IsTouchingCeiling;
        
        private bool _jumpInput;
        private bool _isGrounded;
        private bool _dashInput;

        private bool _normalAttackInput;
        private bool _aimSwordInput;
        private bool _focusSwordInput;
        private bool _counterAttackInput;

        protected GhostTrailController GhostTrailController;
        
        protected PlayerGroundedState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName){}

        // Receive the function the DoChecks() from PlayerState
        // and adding the specific needs for the Grounded State
        protected override void DoChecks()
        {
            base.DoChecks();
            if (CollisionSenses)
            {
                _isGrounded = CollisionSenses.Ground;
                IsTouchingCeiling = CollisionSenses.Ceiling;
            }
        }

        // Before entering the specific state that happen on the ground
        // Player will enter this state first
        public override void Enter()
        {
            base.Enter();
            GhostTrailController = Player.GhostTrailController;
            GhostTrailController.enabled = false;
            
            // When the player standing on the ground,
            // reset the number of jumps and dashes that he can make
            Player.JumpState.ResetAmountOfJumpsLeft();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Get the input values from the PlayerInputHandler class
            XInput = Player.InputHandler.NormInputX;
            YInput = Player.InputHandler.NormInputY;
            _jumpInput =Player.InputHandler.JumpInput;
            _dashInput = Player.InputHandler.DashInput;
            
            _normalAttackInput = Player.InputHandler.NormalAttackInputs[(int)CombatInputs.Normal];
            _aimSwordInput = Player.InputHandler.AimSwordInput;
            _focusSwordInput = Player.InputHandler.FocusSwordInput;
            _counterAttackInput = Player.InputHandler.CounterInput;
            
            if(_focusSwordInput && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.FocusSwordState);
            else if(_aimSwordInput && /*SkillManager.Instance.Slash.CanUseSkill() &&*/ !IsTouchingCeiling)
                StateMachine.ChangeState(Player.AimSwordState);
            else if(_counterAttackInput && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.CounterAttackState);
            else if(_normalAttackInput && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.PrimaryAttackState);
            if(_aimSwordInput && /*SkillManager.Instance.Slash.CanUseSkill() &&*/ !IsTouchingCeiling)
                StateMachine.ChangeState(Player.AimSwordState);
            else if(_counterAttackInput && !IsTouchingCeiling)
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
            // Change to Dash State if there is dashInput and the dash has been cooled down
            else if (_dashInput && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.DashState);
        }
    }
}
