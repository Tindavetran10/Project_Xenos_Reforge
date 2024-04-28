using Controller;
using InventorySystem_and_Items;
using Manager;
using Player.Data;
using Player.PlayerStateMachine;

namespace Player.PlayerStates.SuperStates
{
    // This state will represent all States that happen when the player standing on the ground
    // All of its function going to be inherited from all of those States 
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

        // Before entering the specific state that happens on the ground
        // Player will enter this state first
        public override void Enter()
        {
            base.Enter();
            GhostTrailController = Player.GhostTrailController;
            GhostTrailController.enabled = false;
            
            // When the player standing on the ground,
            // reset the number of jumps and dashes that he can make
            Player.JumpState.ResetAmountOfJumpsLeft();
            Player.DashState.ResetCanDash();
            
            Player.inputManager.UseFlaskEvent += UseFlask;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Get the input values from the PlayerInputHandler class
            XInput = Player.inputManager.NormInputX;
            YInput = Player.inputManager.NormInputY;
            _jumpInput =Player.inputManager.JumpInput;
            _dashInput = Player.inputManager.DashInput;
            
            _normalAttackInput = Player.inputManager.NormalAttackInputs[(int)CombatInputs.Normal];
            _aimSwordInput = Player.inputManager.AimSwordInput;
            _focusSwordInput = Player.inputManager.FocusSwordInput;
            _counterAttackInput = Player.inputManager.CounterInput;
            
            if(_focusSwordInput && SkillManager.instance.Focus.FocusUnlocked && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.FocusSwordState);
            else if(_aimSwordInput && SkillManager.instance.Slash.SlashUnlocked 
                        && SkillManager.instance.Slash.CanUseSkill() && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.AimSwordState);
            else if(_counterAttackInput && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.CounterAttackState);
            else if(_normalAttackInput && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.PrimaryAttackState);
            else if(_counterAttackInput && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.CounterAttackState);
            else if(_normalAttackInput && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.PrimaryAttackState);
            
            // Change to Jump State if there is a jumpInput, the number of jumps is > 0,
            // and he isn't touch the ceiling 
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
            else if (_dashInput && SkillManager.instance.Dash.DashUnlocked 
                        && Player.DashState.CheckIfCanDash() && !IsTouchingCeiling)
                StateMachine.ChangeState(Player.DashState);
        }
        
        public override void Exit()
        {
            base.Exit();
            Player.inputManager.UseFlaskEvent -= UseFlask;
        }
        
        private static void UseFlask() => InventoryManager.instance.UseFlask();
    }
}
