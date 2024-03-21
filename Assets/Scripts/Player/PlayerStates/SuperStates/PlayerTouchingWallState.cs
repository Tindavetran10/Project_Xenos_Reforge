using Player.Data;
using Player.PlayerStateMachine;

namespace Player.PlayerStates.SuperStates
{
    public class PlayerTouchingWallState : PlayerState
    {
        #region Checks
        private bool _isGrounded;
        private bool _isTouchingWall;
        private bool _isTouchingLedge;
        private bool _jumpInput;
        private int _xInput;
        protected int YInput;
        #endregion

        protected PlayerTouchingWallState(global::Player.PlayerStateMachine.Player player, global::Player.PlayerStateMachine.PlayerStateMachine stateMachine, 
            PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {}


        protected override void DoChecks()
        {
            base.DoChecks();
            
            if (CollisionSenses)
            {
                _isGrounded = CollisionSenses.Ground;
                _isTouchingWall = CollisionSenses.WallFront;
                _isTouchingLedge = CollisionSenses.LedgeHorizontal;
            }
            
            if (_isTouchingWall && !_isTouchingLedge) 
                Player.LedgeClimbState.SetDetectedPosition(Player.transform.position);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _xInput = Player.playerInputHandler.NormInputX;
            YInput = Player.playerInputHandler.NormInputY;
            _jumpInput = Player.playerInputHandler.JumpInput;

            if (_jumpInput)
            {
                Player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
                StateMachine.ChangeState(Player.WallJumpState);
            }
            else if (_isGrounded)
                StateMachine.ChangeState(Player.IdleState);
            else if (!_isTouchingWall || _xInput != Movement?.FacingDirection)
                StateMachine.ChangeState(Player.InAirState);
            else if (_isTouchingWall && !_isTouchingLedge)
                StateMachine.ChangeState(Player.LedgeClimbState);
        }
    }
}
